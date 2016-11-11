using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class RewardController : DataEvaluationController
	{
		// todo change all db controller usage to core controller usage
		public RewardController(Data.EntityFramework.Controllers.GameDataController gameDataController, GroupRelationshipController groupRelationshipController, UserRelationshipController userRelationshipController)
			: base(gameDataController, groupRelationshipController, userRelationshipController)
		{
		}

		public bool AddReward(int? actorId, int? gameId, Reward reward)
		{
			var gameData = new Data.Model.GameData()
			{
				Key = reward.Key,
				GameId = gameId,    //TODO: handle the case where a global achievement has been completed for a specific game
				ActorId = actorId,
				DataType = reward.DataType,
				Value = reward.Value
			};
			GameDataController.Create(gameData);
			return true;
		}
	}
}