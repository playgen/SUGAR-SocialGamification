using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using System;
using PlayGen.SUGAR.GameData;
using Xunit;
using AchievementController = PlayGen.SUGAR.Data.EntityFramework.Controllers.AchievementController;
using LeaderboardController = PlayGen.SUGAR.Data.EntityFramework.Controllers.LeaderboardController;
using SkillController = PlayGen.SUGAR.Data.EntityFramework.Controllers.SkillController;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class TestEnvironment : IDisposable
	{
		private readonly string _dbName = "sgaunittests";
		private readonly string _connectionString;

		private AccountController _accountController;
		private AchievementController _achievementController;
		private ActorController _actorController;
		private GameController _gameController;
		private GameDataController _gameDataController;
		private GroupController _groupController;
		private GroupRelationshipController _groupRelationshipController;
		private LeaderboardController _leaderboardController;
		private ResourceController _resourceController;
		private SkillController _skillController;
		private UserController _userController;
		private UserRelationshipController _userRelationshipController;

		public AccountController AccountController
			=> _accountController ?? (_accountController = new AccountController(_connectionString));
		public AchievementController AchievementController
			=> _achievementController ?? (_achievementController = new AchievementController(_connectionString));
		public ActorController ActorController
			=> _actorController ?? (_actorController = new ActorController(_connectionString));
		public GameController GameController 
			=> _gameController ?? (_gameController = new GameController(_connectionString));
		public GameDataController GameDataController 
			=> _gameDataController ?? (_gameDataController = new GameDataController(_connectionString));
		public GroupController GroupController
			=> _groupController ?? (_groupController = new GroupController(_connectionString));
		public GroupRelationshipController GroupRelationshipController
			=>_groupRelationshipController ?? (_groupRelationshipController = new GroupRelationshipController(_connectionString));
		public LeaderboardController LeaderboardController
			=> _leaderboardController ?? (_leaderboardController = new LeaderboardController(_connectionString));
		public ResourceController ResourceController 
			=> _resourceController ?? (_resourceController = new ResourceController(_connectionString));
		public SkillController SkillController
			=> _skillController ?? (_skillController = new SkillController(_connectionString));
		public UserController UserController 
			=> _userController ?? (_userController = new UserController(_connectionString));
		public UserRelationshipController UserRelationshipController
			=> _userRelationshipController ?? (_userRelationshipController = new UserRelationshipController(_connectionString));

		public TestEnvironment()
		{
			_connectionString = GetNameOrConnectionString(_dbName);
			DeleteDatabase();
		}

		private string GetNameOrConnectionString(string dbName)
		{
			return "Server=localhost;" +
					"Port=3306;" +
					$"Database={dbName};" +
					"Uid=root;" +
					"Pwd=t0pSECr3t;" +
					"Convert Zero Datetime=true;" +
					"Allow Zero Datetime=true";
		}

		private void DeleteDatabase()
		{
			using (var context = new SUGARContext(_connectionString))
			{
				if (context.Database.Connection.Database == _dbName)
				{
					context.Database.Delete();
				}
				else
				{
					throw new Exception($"Database with name: {_dbName} doesn't exist.");
				}
			}
		}
		
		public void Dispose()
		{
		}
	}
}