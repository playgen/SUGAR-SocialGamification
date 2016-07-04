using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;

namespace PlayGen.SUGAR.GameData
{
	public class RewardController : DataEvaluationController
	{
		public RewardController(IGameDataController gameDataController, GroupRelationshipController groupRelationshipController)
			: base(gameDataController, groupRelationshipController)
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