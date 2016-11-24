using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Core.Utilities;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class EvaluationController : CriteriaEvaluator
	{
	    public static Action<Evaluation> EvaluationCreatedEvent;
        public static Action<Evaluation> EvaluationUpdatedEvent;
        public static Action<Evaluation> EvaluationDeletedEvent;

        private readonly RewardController _rewardController;
		private readonly ActorController _actorController;
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
			ActorController actorController,
			RewardController rewardController)
			: base(gameDataCoreController, groupMemberCoreController, userFriendCoreController)
		{
			_evaluationDbController = evaluationDbController;
			_rewardController = rewardController;
			_actorController = actorController;
		}

        public IEnumerable<Evaluation> All()
        {
            var evaluations = _evaluationDbController.All();
            return evaluations;
        }

        public Evaluation Get(string token, int? gameId)
		{
			var evaluation = _evaluationDbController.Get(token, gameId);
			return evaluation;
		}
		
		public IEnumerable<Evaluation> GetByGame(int? gameId)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId);
			return evaluations;
		}
		
		public IEnumerable<EvaluationProgress> GetGameProgress(int gameId, int? actorId)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId);
			evaluations = FilterByActorType(evaluations, actorId);

			var evaluationsProgress = evaluations.Select(e => new EvaluationProgress
			{
				Name = e.Name,
				Progress = EvaluateProgress(e, actorId),
			});

			return evaluationsProgress;
		}
	   
		public EvaluationProgress GetProgress(string token, int? gameId, int actorId)
		{
			var evaluation = _evaluationDbController.Get(token, gameId);
			var progress = EvaluateProgress(evaluation, actorId);

			return new EvaluationProgress
			{
                Actor = _actorController.Get(actorId),
				Name = evaluation.Name,
				Progress = progress,
			};
		}
		
		public Evaluation Create(Evaluation evaluation)
		{
			evaluation = _evaluationDbController.Create(evaluation);

            EvaluationCreatedEvent?.Invoke(evaluation);
            return evaluation;
		}
		
		public void Update(Evaluation evaluation)
		{
			_evaluationDbController.Update(evaluation);

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
		}

		public IEnumerable<Evaluation> FilterByActorType(IEnumerable<Evaluation> evaluations, int? actorId)
		{
			if (actorId.HasValue)
			{
			    var provided = _actorController.Get(actorId.Value);
			    evaluations = provided == null ? evaluations.Where(a => a.ActorType == ActorType.Undefined) :
                                                evaluations.Where(a => a.ActorType == ActorType.Undefined || a.ActorType == provided.ActorType);
			}

			return evaluations;
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

			var key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token);
			var completed = GameDataCoreController.KeyExists(evaluation.GameId, actorId, key);
			var completedProgress = completed ? 1f : 0f;
			if (!completed)
			{
				completedProgress = IsCriteriaSatisified(evaluation.GameId, actorId, evaluation.EvaluationCriterias, evaluation.ActorType);
				if (completedProgress >= 1)
				{
					ProcessEvaluationRewards(evaluation, actorId);
				}
			}

			return completedProgress;
		}

        private void ProcessEvaluationRewards(Evaluation evaluation, int? actorId)
		{
			var gameData = new GameData()
			{
				Key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token),
				GameId = evaluation.GameId,    //TODO: handle the case where a global evaluation has been completed for a specific game
				ActorId = actorId,
				SaveDataType = SaveDataType.String,
				Value = null
			};
			GameDataCoreController.Add(gameData);
			evaluation.Rewards.ForEach(reward => _rewardController.AddReward(actorId, evaluation.GameId, reward));
		}
	}
}