using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

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
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, reward.EvaluationDataCategory);

			var evaluationData = new EvaluationData {
				Key = reward.EvaluationDataKey,
				GameId = gameId,
				ActorId = actorId,
				Category = reward.EvaluationDataCategory,
				EvaluationDataType = reward.EvaluationDataType,
				Value = reward.Value
			};

			evaluationDataController.Add(evaluationData);

			_logger.LogInformation($"Game Data: {evaluationData.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward.Id}");

			return true;
		}
	}
}