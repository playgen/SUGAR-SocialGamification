using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;
using System;
using System.Linq;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RewardController : CriteriaEvaluator
	{
		private readonly ILogger _logger;

		public RewardController(
			ILogger<RewardController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory,
			RelationshipController relationshipController)
			: base(evaluationDataLogger, contextFactory, relationshipController)
		{
			_logger = logger;
		}

		public bool AddReward(int actorId, int gameId, Reward reward)
		{
			var evaluationDataController = new EvaluationDataController(ContextFactory, reward.EvaluationDataCategory);
			
			var foundResources = evaluationDataController.Get(gameId, actorId, new[] { reward.EvaluationDataKey });

			if (foundResources.Any())
			{
				EvaluationData resource = foundResources.Single();

				var currentValue = long.Parse(resource.Value);
				var addedValue = long.Parse(reward.Value);
				resource.Value = (currentValue + addedValue).ToString();

				evaluationDataController.Update(resource);

				Logger.Info($"{resource?.Id} with Amount: {addedValue}");


			}
			else
			{
				var evaluationData = new EvaluationData {
					Key = reward.EvaluationDataKey,
					GameId = gameId,    //TODO: handle the case where a global achievement has been completed for a specific game
					ActorId = actorId,
					Category = reward.EvaluationDataCategory,
					EvaluationDataType = reward.EvaluationDataType,
					Value = reward.Value
				};
				
				evaluationDataController.Add(evaluationData);

				Logger.Info($"Game Data: {evaluationData?.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward?.Id}");

			}
			return true;
		}
	}
}