using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Core;
using PlayGen.SUGAR.Core.Controllers;
using EvaluationController = PlayGen.SUGAR.Core.Controllers.EvaluationController;
using LeaderboardController = PlayGen.SUGAR.Data.EntityFramework.Controllers.LeaderboardController;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public abstract class ControllerLocator
	{
		private static Controllers.AccountController _accountController;
		private static ActorController _actorController;
		private static Controllers.GameController _gameController;
		private static Controllers.GameDataController _gameDataController;
		private static Controllers.GroupController _groupController;
		private static GroupRelationshipController _groupRelationshipController;
		private static LeaderboardController _leaderboardController;
		private static ResourceController _resourceController;
		private static EntityFramework.Controllers.EvaluationController _evaluationController;
		private static Controllers.UserController _userController;
		private static UserRelationshipController _userRelationshipController;

		public static Controllers.AccountController AccountController
			=> _accountController ?? (_accountController = new Controllers.AccountController(ProjectFixture.ContextFactory));
		public static EntityFramework.Controllers.EvaluationController EvaluationController
			=> _evaluationController ?? (_evaluationController = new EntityFramework.Controllers.EvaluationController(ProjectFixture.ContextFactory));
		public static  ActorController ActorController
			=> _actorController ?? (_actorController = new ActorController(ProjectFixture.ContextFactory));
		public static Controllers.GameController GameController 
			=> _gameController ?? (_gameController = new Controllers.GameController(ProjectFixture.ContextFactory));
		public static Controllers.GameDataController GameDataController 
			=> _gameDataController ?? (_gameDataController = new Controllers.GameDataController(ProjectFixture.ContextFactory));
		public static Controllers.GroupController GroupController
			=> _groupController ?? (_groupController = new Controllers.GroupController(ProjectFixture.ContextFactory));
		public static  GroupRelationshipController GroupRelationshipController
			=>_groupRelationshipController ?? (_groupRelationshipController = new GroupRelationshipController(ProjectFixture.ContextFactory));
		public static  LeaderboardController LeaderboardController
			=> _leaderboardController ?? (_leaderboardController = new LeaderboardController(ProjectFixture.ContextFactory));
		public static  ResourceController ResourceController 
			=> _resourceController ?? (_resourceController = new ResourceController(ProjectFixture.ContextFactory));
		public static Controllers.UserController UserController 
			=> _userController ?? (_userController = new Controllers.UserController(ProjectFixture.ContextFactory));
		public static  UserRelationshipController UserRelationshipController
			=> _userRelationshipController ?? (_userRelationshipController = new UserRelationshipController(ProjectFixture.ContextFactory));
	}
}