using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserDataControllerTests : TestController
	{
		#region Configuration
		private readonly UserController _userDbController;
		private readonly GameController _gameDbController;
		private readonly GameDataController _userDataDbController;

		public UserDataControllerTests()
		{
			InitializeEnvironment();

			_userDbController = new UserController(TestController.NameOrConnectionString);
			_gameDbController = new GameController(TestController.NameOrConnectionString);
			_userDataDbController = new GameDataController(NameOrConnectionString);
		}

		private void InitializeEnvironment()
		{
			TestController.DeleteDatabase();
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserSaveData()
		{
			string userDataName = "CreateUserData";

			var newSaveData = CreateUserData(userDataName);

			var userDatas = _userDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			int matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateUserDataWithNonExistingGame()
		{
			string userDataName = "CreateUserDataWithNonExistingGame";

			Assert.Throws<MissingRecordException>(() => CreateUserData(userDataName, -1));
		}

		[Fact]
		public void CreateUserDataWithNonExistingUser()
		{
			string userDataName = "CreateUserDataWithNonExistingUser";

			Assert.Throws<MissingRecordException>(() => CreateUserData(userDataName, 0, -1));
		}

		[Fact]
		public void GetMultipleUserSaveDatas()
		{
			string[] userDataNames = new[]
			{
				"GetMultipleUserSaveDatas1",
				"GetMultipleUserSaveDatas2",
				"GetMultipleUserSaveDatas3",
				"GetMultipleUserSaveDatas4",
			};

			var doNotFind = CreateUserData("GetMultipleUserSaveDatas_DontGetThis");
			var gameId = doNotFind.GameId;
			var userId = doNotFind.ActorId;

			foreach (var userDataName in userDataNames)
			{
				CreateUserData(userDataName, gameId.Value, userId.Value);
			}

			var userDatas = _userDataDbController.Get(gameId, userId, userDataNames);

			var matchingUserSaveDatas = userDatas.Select(g => userDataNames.Contains(g.Key));

			Assert.Equal(matchingUserSaveDatas.Count(), userDataNames.Length);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingKey()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingKey";

			var newSaveData = CreateUserData(userDataName);

			var userDatas = _userDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { "null key" });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingGame()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingGame";

			var newSaveData = CreateUserData(userDataName);

			var userDatas = _userDataDbController.Get(-1, newSaveData.ActorId, new string[] { userDataName });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingUser()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingUser";

			var newSaveData = CreateUserData(userDataName);

			var userDatas = _userDataDbController.Get(newSaveData.GameId, -1, new string[] { userDataName });

			Assert.Empty(userDatas);
		}
		#endregion

		#region Helpers
		private GameData CreateUserData(string key, int gameId = 0, int userId = 0)
		{
			GameController gameDbController = new GameController(NameOrConnectionString);
			if (gameId == 0)
			{
				Game game = new Game
				{
					Name = key
				};
				gameDbController.Create(game);
				gameId = game.Id;
			}

			UserController userDbController = new UserController(NameOrConnectionString);
			if (userId == 0)
			{
				var user = new User
				{
					Name = key
				};
				userDbController.Create(user);
				userId = user.Id;
			}

			var userData = new GameData
			{
				Key = key,
				GameId = gameId,
				ActorId = userId,
				Value = key + " value",
				DataType = 0
			};
			_userDataDbController.Create(userData);
			return userData;
		}
		#endregion
	}
}
