using NLog;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RewardController : CriteriaEvaluator
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        // todo change all db controller usage to core controller usage
        public RewardController(GameDataController gameDataCoreController, 
            GroupMemberController groupMemberCoreController, 
            UserFriendController userFriendCoreController)
			: base(gameDataCoreController, groupMemberCoreController, userFriendCoreController)
		{
		}

		public bool AddReward(int? actorId, int? gameId, Reward reward)
		{
			var gameData = new GameData()
			{
				Key = reward.Key,
				GameId = gameId,    //TODO: handle the case where a global achievement has been completed for a specific game
				ActorId = actorId,
				SaveDataType = reward.DataType,
				Value = reward.Value
			};
			GameDataCoreController.Add(gameData);

            Logger.Info($"Game Data: {gameData?.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward?.Id}");

			return true;
		}
	}
}