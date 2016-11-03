using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.GameData;
using AchievementController = PlayGen.SUGAR.Data.EntityFramework.Controllers.AchievementController;
using LeaderboardController = PlayGen.SUGAR.Data.EntityFramework.Controllers.LeaderboardController;
using SkillController = PlayGen.SUGAR.Data.EntityFramework.Controllers.SkillController;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public abstract class ControllerLocator
	{
		private static AccountController _accountController;
		private static AchievementController _achievementController;
		private static ActorController _actorController;
		private static GameController _gameController;
		private static GameDataController _gameDataController;
		private static GroupController _groupController;
		private static GroupRelationshipController _groupRelationshipController;
		private static LeaderboardController _leaderboardController;
		private static ResourceController _resourceController;
		private static SkillController _skillController;
		private static UserController _userController;
		private static UserRelationshipController _userRelationshipController;

		public static  AccountController AccountController
			=> _accountController ?? (_accountController = new AccountController(ProjectFixture.ContextFactory));
		public static  AchievementController AchievementController
			=> _achievementController ?? (_achievementController = new AchievementController(ProjectFixture.ContextFactory));
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
		public static  SkillController SkillController
			=> _skillController ?? (_skillController = new SkillController(ProjectFixture.ContextFactory));
		public static  UserController UserController 
			=> _userController ?? (_userController = new UserController(ProjectFixture.ContextFactory));
		public static  UserRelationshipController UserRelationshipController
			=> _userRelationshipController ?? (_userRelationshipController = new UserRelationshipController(ProjectFixture.ContextFactory));
	}
}