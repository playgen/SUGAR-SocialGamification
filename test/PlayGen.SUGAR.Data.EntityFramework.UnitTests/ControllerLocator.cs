using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Gore;
using EvaluationController = PlayGen.SUGAR.Gore.EvaluationController;
using LeaderboardController = PlayGen.SUGAR.Data.EntityFramework.Controllers.LeaderboardController;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public abstract class ControllerLocator
	{
		private static AccountController _accountController;
		private static ActorController _actorController;
		private static GameController _gameController;
		private static GameDataController _gameDataController;
		private static GroupController _groupController;
		private static GroupRelationshipController _groupRelationshipController;
		private static LeaderboardController _leaderboardController;
		private static ResourceController _resourceController;
		private static EntityFramework.Controllers.EvaluationController _evaluationController;
		private static UserController _userController;
		private static UserRelationshipController _userRelationshipController;

		public static  AccountController AccountController
			=> _accountController ?? (_accountController = new AccountController(ProjectFixture.ContextFactory));
		public static EntityFramework.Controllers.EvaluationController EvaluationController
			=> _evaluationController ?? (_evaluationController = new EntityFramework.Controllers.EvaluationController(ProjectFixture.ContextFactory));
		public static  ActorController ActorController
			=> _actorController ?? (_actorController = new ActorController(ProjectFixture.ContextFactory));
		public static  GameController GameController 
			=> _gameController ?? (_gameController = new GameController(ProjectFixture.ContextFactory));
		public static  GameDataController GameDataController 
			=> _gameDataController ?? (_gameDataController = new GameDataController(ProjectFixture.ContextFactory));
		public static  GroupController GroupController
			=> _groupController ?? (_groupController = new GroupController(ProjectFixture.ContextFactory));
		public static  GroupRelationshipController GroupRelationshipController
			=>_groupRelationshipController ?? (_groupRelationshipController = new GroupRelationshipController(ProjectFixture.ContextFactory));
		public static  LeaderboardController LeaderboardController
			=> _leaderboardController ?? (_leaderboardController = new LeaderboardController(ProjectFixture.ContextFactory));
		public static  ResourceController ResourceController 
			=> _resourceController ?? (_resourceController = new ResourceController(ProjectFixture.ContextFactory));
		public static  UserController UserController 
			=> _userController ?? (_userController = new UserController(ProjectFixture.ContextFactory));
		public static  UserRelationshipController UserRelationshipController
			=> _userRelationshipController ?? (_userRelationshipController = new UserRelationshipController(ProjectFixture.ContextFactory));
	}
}