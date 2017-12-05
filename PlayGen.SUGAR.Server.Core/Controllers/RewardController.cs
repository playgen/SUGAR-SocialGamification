using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public class RewardController : CriteriaEvaluator
	{
		private readonly ILogger _logger;

		public RewardController(
			ILogger<RewardController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory,
			GroupMemberController groupMemberCoreController,
			UserFriendController userFriendCoreController)
			: base(evaluationDataLogger, contextFactory, groupMemberCoreController, userFriendCoreController)
		{
			_logger = logger;
		}

		public bool AddReward(int actorId, int gameId, Reward reward)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, reward.EvaluationDataCategory.Value);

			var evaluationData = new EvaluationData {
				Key = reward.EvaluationDataKey,
				GameId = gameId,
				ActorId = actorId,
				Category = reward.EvaluationDataCategory.Value,
				EvaluationDataType = reward.EvaluationDataType.Value,
				Value = reward.Value
			};

			evaluationDataController.Add(evaluationData);

			_logger.LogInformation($"Game Data: {evaluationData.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward.Id}");

			return true;
		}
	}
}