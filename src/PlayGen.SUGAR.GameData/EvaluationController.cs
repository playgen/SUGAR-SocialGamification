using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;


namespace PlayGen.SUGAR.GameData
{
	public class EvaluationController : DataEvaluationController
	{
		protected readonly RewardController RewardController;
		protected readonly ActorController ActorController;

	    private readonly Dictionary<EvaluationType, string> _evaluationFormatMappings = new Dictionary<EvaluationType, string>
	    {
	        {EvaluationType.Achievement, KeyConstants.AchievementCompleteFormat},
	        {EvaluationType.Skill, KeyConstants.SkillCompleteFormat},
	    };

        public EvaluationController(GameDataController gameDataController,
			GroupRelationshipController groupRelationshipController,
			UserRelationshipController userRelationshipController,
			ActorController actorController,
			RewardController rewardController)
			: base(gameDataController, groupRelationshipController, userRelationshipController)
		{
			RewardController = rewardController;
			ActorController = actorController;
		}

		public IEnumerable<Evaluation> FilterByActorType(IEnumerable<Evaluation> evaluations, int? actorId)
		{
			if (actorId.HasValue)
			{
				var provided = ActorController.Get(actorId.Value);
				if (provided == null)
				{
					evaluations = evaluations.Where(a => a.ActorType == ActorType.Undefined);
				}
				else
				{
					evaluations = evaluations.Where(a => a.ActorType == ActorType.Undefined || a.ActorType == provided.ActorType);
				}
			}

			return evaluations;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="evaluation"></param>
		/// <param name="actorId"></param>
		/// <returns></returns>
		public float IsEvaluationCompleted(Evaluation evaluation, int? actorId)
		{
			if (evaluation == null)
			{
				throw new MissingRecordException("The provided evaluation does not exist.");
			}
			if (actorId != null)
			{
				var provided = ActorController.Get(actorId.Value);
				if (evaluation.ActorType != ActorType.Undefined && (provided == null || provided.ActorType != evaluation.ActorType))
				{
					throw new MissingRecordException("The provided ActorId cannot complete this evaluation.");
				}
			}

            var key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token);
			var completed = GameDataController.KeyExists(evaluation.GameId, actorId, key);
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
			var gameData = new Data.Model.GameData()
			{
				Key = string.Format(_evaluationFormatMappings[evaluation.EvaluationType], evaluation.Token),
				GameId = evaluation.GameId,    //TODO: handle the case where a global evaluation has been completed for a specific game
				ActorId = actorId,
				DataType = GameDataType.String,
				Value = null
			};
			GameDataController.Create(gameData);
			evaluation.Rewards.All(reward => RewardController.AddReward(actorId, evaluation.GameId, reward));
		}

		public void EvaluateEvaluation(Evaluation evaluation, int? actorId)
		{
		}
	}
}