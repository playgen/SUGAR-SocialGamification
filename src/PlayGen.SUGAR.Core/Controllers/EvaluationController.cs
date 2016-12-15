using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class EvaluationController : CriteriaEvaluator
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static Action<Evaluation> EvaluationCreatedEvent;
		public static Action<Evaluation> EvaluationUpdatedEvent;
		public static Action<Evaluation> EvaluationDeletedEvent;

		private readonly RewardController _rewardController;
		private readonly Data.EntityFramework.Controllers.ActorController _actorController;
		private readonly Data.EntityFramework.Controllers.EvaluationController _evaluationDbController;

		private readonly Dictionary<EvaluationType, string> _evaluationFormatMappings = new Dictionary<EvaluationType, string>
		{
			{EvaluationType.Achievement, KeyConstants.AchievementCompleteFormat},
			{EvaluationType.Skill, KeyConstants.SkillCompleteFormat},
		};

		// todo change all db controller usages to core controller usages except for evaluation db controller
		public EvaluationController(Data.EntityFramework.Controllers.EvaluationController evaluationDbController,
			GameDataController gameDataCoreController,
			GroupMemberController groupMemberCoreController,
			UserFriendController userFriendCoreController,
			Data.EntityFramework.Controllers.ActorController actorController,
			RewardController rewardController)
			: base(gameDataCoreController, groupMemberCoreController, userFriendCoreController)
		{
			_evaluationDbController = evaluationDbController;
			_rewardController = rewardController;
			_actorController = actorController;
		}

		public List<Evaluation> Get()
		{
			var evaluations = _evaluationDbController.Get();

            Logger.Info($"{evaluations?.Count}");

			return evaluations;
		}

		public List<Evaluation> GetByGame(int? gameId)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId);

            Logger.Info($"{evaluations?.Count} Evalautions for GameId: {gameId}");

            return evaluations;
		}

		public Evaluation Get(string token, int? gameId)
		{
			var evaluation = _evaluationDbController.Get(token, gameId);

            Logger.Info($"Evalaution: {evaluation?.Id} for Token: {token}, GameId: {gameId}");

            return evaluation;
		}
		
		public List<EvaluationProgress> GetGameProgress(int gameId, int? actorId)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId);
			evaluations = FilterByActorType(evaluations, actorId);

			var evaluationsProgress = evaluations.Select(e => new EvaluationProgress
			{
				Actor = _actorController.Get(actorId.Value),
				Name = e.Name,
				Progress = EvaluateProgress(e, actorId),
			}).ToList();

            Logger.Info($"{evaluationsProgress?.Count} Evaluation Progresses for GameId: {gameId}, ActorId: {actorId}");

            return evaluationsProgress;
		}
	   
		public EvaluationProgress GetProgress(string token, int? gameId, int actorId)
		{
			var evaluation = _evaluationDbController.Get(token, gameId);
			var progress = EvaluateProgress(evaluation, actorId);

            var result = new EvaluationProgress
            {
                Actor = _actorController.Get(actorId),
                Name = evaluation.Name,
                Progress = progress,
            };

            Logger.Info( $"{result?.Name} Evaluation Progresses for Token: {token}, GameId: {gameId}, ActorId: {actorId}");

		    return result;
		}
		
		public Evaluation Create(Evaluation evaluation)
		{
			foreach (var ec in evaluation.EvaluationCriterias)
			{
				if (!DataTypeValueValidation(ec.DataType, ec.Value))
				{
					throw new InvalidCastException($"{ec.Value} cannot be cast to DataType {ec.DataType}");
				}
			}
			evaluation = _evaluationDbController.Create(evaluation);

			EvaluationCreatedEvent?.Invoke(evaluation);

            Logger.Info($"{evaluation?.Id}");

            return evaluation;
		}
		
		public void Update(Evaluation evaluation)
		{
			foreach (var ec in evaluation.EvaluationCriterias)
			{
				if (!DataTypeValueValidation(ec.DataType, ec.Value))
				{
					throw new InvalidCastException($"{ec.Value} cannot be cast to DataType {ec.DataType}");
				}
			}
			_evaluationDbController.Update(evaluation);

            Logger.Info($"{evaluation?.Id}");

            EvaluationUpdatedEvent?.Invoke(evaluation);
		}
		
		public void Delete(string token, int? gameId)
		{
			var evaluation = Get(token, gameId);

			if (evaluation == null)
			{
				throw new MissingRecordException($"The evaluation with token \"{token}\" for gameId {gameId} cannot be found.");
			}

			EvaluationDeletedEvent?.Invoke(evaluation);
			_evaluationDbController.Delete(token, gameId);

            Logger.Info($"Deleted: {evaluation?.Id} for Token {token}, GameId: {gameId}");
        }

		private bool DataTypeValueValidation(SaveDataType dataType, string value)
		{
			switch (dataType)
			{
				case SaveDataType.String:
					return true;
				case SaveDataType.Long:
					long longValue;
					return long.TryParse(value, out longValue);
				case SaveDataType.Float:
					float floatValue;
					return float.TryParse(value, out floatValue);
				case SaveDataType.Boolean:
					bool boolValue;
					return bool.TryParse(value, out boolValue);
				default:
					return false;
			}
		}
		
		public float EvaluateProgress(Evaluation evaluation, int? actorId)
		{
			if (evaluation == null)
			{
				throw new MissingRecordException("The provided evaluation does not exist.");
			}
			if (actorId != null)
			{
				var provided = _actorController.Get(actorId.Value);
				if (evaluation.ActorType != ActorType.Undefined && (provided == null || provided.ActorType != evaluation.ActorType))
				{
					throw new MissingRecordException("The provided ActorId cannot complete this evaluation.");
				}
			}

			var completed = IsAlreadyCompleted(evaluation, actorId.Value);
			var completedProgress = completed ? 1f : 0f;
			
			if (!completed)
			{
				completedProgress = IsCriteriaSatisified(evaluation.GameId, actorId, evaluation.EvaluationCriterias, evaluation.ActorType);
				if (completedProgress >= 1)
				{
					ProcessEvaluationRewards(evaluation, actorId);
				}
			}

            Logger.Debug($"Got: Progress: {completedProgress} for Evaluation.Id: {evaluation?.Id}, ActorId: {actorId}");

            return completedProgress;
		}

		public bool IsAlreadyCompleted(Evaluation evaluation, int actorId)
		{
			var key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token);
			var completed = GameDataCoreController.KeyExists(evaluation.GameId, actorId, key);

            Logger.Debug($"Got: IsCompleted: {completed} for Evaluation.Id: {evaluation?.Id}, ActorId: {actorId}");

            return completed;
		}

		private void ProcessEvaluationRewards(Evaluation evaluation, int? actorId)
		{
			var gameData = new EvaluationData()
			{
				Key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token),
				GameId = evaluation.GameId,    //TODO: handle the case where a global evaluation has been completed for a specific game
				ActorId = actorId,
				SaveDataType = SaveDataType.String,
				Value = null
			};
			GameDataCoreController.Add(gameData);

			evaluation.Rewards?.ForEach(reward => _rewardController.AddReward(actorId, evaluation.GameId, reward));
		}

        private List<Evaluation> FilterByActorType(List<Evaluation> evaluations, int? actorId)
        {
            if (actorId.HasValue)
            {
                var provided = _actorController.Get(actorId.Value);
                evaluations = provided == null
                    ? evaluations.Where(a => a.ActorType == ActorType.Undefined).ToList()
                    : evaluations.Where(a => a.ActorType == ActorType.Undefined || a.ActorType == provided.ActorType).ToList();
            }

            return evaluations;
        }
    }
}