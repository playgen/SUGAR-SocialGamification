using NLog;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RewardController : CriteriaEvaluator
	{
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public RewardController(SUGARContextFactory contextFactory, 
            GroupMemberController groupMemberCoreController, 
            UserFriendController userFriendCoreController)
			: base(contextFactory, groupMemberCoreController, userFriendCoreController)
		{
		}

		public bool AddReward(int? actorId, int? gameId, Reward reward)
		{
		    var saveDataController = new SaveDataController(ContextFactory, reward.SaveDataCategory);

			var saveData = new SaveData
			{
				Key = reward.SaveDataKey,
				GameId = gameId,    //TODO: handle the case where a global achievement has been completed for a specific game
				ActorId = actorId,
                Category = reward.SaveDataCategory,
				SaveDataType = reward.SaveDataType,
				Value = reward.Value
			};

            saveDataController.Add(saveData);

            Logger.Info($"Game Data: {saveData?.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward?.Id}");

			return true;
		}
	}
}