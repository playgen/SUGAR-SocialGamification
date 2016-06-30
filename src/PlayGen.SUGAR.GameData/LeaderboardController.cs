using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.GameData
{
	public class LeaderboardController : DataEvaluationController
	{
		protected readonly ActorController ActorController;

		public LeaderboardController(IGameDataController gameDataController,
			GroupRelationshipController groupRelationshipController,
			ActorController actorController)
			: base(gameDataController, groupRelationshipController)
		{
			ActorController = actorController;
		}
	}
}