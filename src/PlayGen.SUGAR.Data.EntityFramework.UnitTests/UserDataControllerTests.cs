using System;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using Xunit;
using System.IO;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GameDataControllerTests : TestController
	{
		#region Configuration
		private readonly UserController _userDbController;
		private readonly GameController _gameDbController;
		private readonly GameDataController _gameDataDbController;

		public GameDataControllerTests()
		{
			_userDbController = new UserController(TestController.NameOrConnectionString);
			_gameDbController = new GameController(TestController.NameOrConnectionString);
			_gameDataDbController = new GameDataController(NameOrConnectionString);

			File.Create(@"C:\Users\Jared\Documents\" + DateTime.UtcNow.Ticks.ToString()).Dispose();
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserGameSaveData()
		{
			string userDataName = "CreateAndGetUserGameSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true, createNewUser: true);

			var userDatas = _gameDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			int matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetUserGlobalSaveData()
		{
			string userDataName = "CreateAndGetUserGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewUser: true);

			var userDatas = _gameDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			int matches = userDatas.Count(g => g.Key == userDataName && g.GameId == null && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetGameGlobalSaveData()
		{
			string userDataName = "CreateAndGetGameGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true);

			var userDatas = _gameDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			int matches = userDatas.Count(g => g.Key == newSaveData.Key && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
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

			var doNotFind = CreateGameData("GetMultipleUserSaveDatas_DontGetThis");
			var gameId = doNotFind.GameId;
			var userId = doNotFind.ActorId;

			foreach (var userDataName in userDataNames)
			{
				CreateGameData(userDataName, gameId, userId);
			}

			var userDatas = _gameDataDbController.Get(gameId, userId, userDataNames);

			var matchingUserSaveDatas = userDatas.Select(g => userDataNames.Contains(g.Key));

			Assert.Equal(matchingUserSaveDatas.Count(), userDataNames.Length);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingKey()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingKey";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { "null key" });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingGame()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingGame";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataDbController.Get(-1, newSaveData.ActorId, new string[] { userDataName });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingUser()
		{
			string userDataName = "GetUserSaveDatasWithNonExistingUser";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataDbController.Get(newSaveData.GameId, -1, new string[] { userDataName });

			Assert.Empty(userDatas);
		}
		#endregion

		#region Helpers
		private GameData CreateGameData(string key, int? gameId = null, int? userId = null, 
			bool createNewGame = false, bool createNewUser = false)
		{
			if (createNewGame)
			{
				GameController gameDbController = new GameController(NameOrConnectionString);
				Game game = new Game
				{
					Name = key
				};
				gameDbController.Create(game);
				gameId = game.Id;
			}

			if (createNewUser)
			{
				UserController userDbController = new UserController(NameOrConnectionString);				
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
			_gameDataDbController.Create(userData);
			return userData;
		}
		#endregion
	}
}
