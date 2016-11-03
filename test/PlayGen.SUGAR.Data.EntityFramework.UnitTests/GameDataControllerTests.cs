using System;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using NUnit.Framework;
using System.IO;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GameDataControllerTests
	{
		#region Configuration
		private readonly GameDataController _gameDataController;
		private readonly GameController _gameController;
		private readonly UserController _userController;

		public GameDataControllerTests()
		{
			_gameDataController = TestEnvironment.GameDataController;
			_gameController = TestEnvironment.GameController;
			_userController = TestEnvironment.UserController;
		}
		#endregion
		
		#region Tests
		[Test]
		public void CreateAndGetUserGameSaveData()
		{
			var userDataName = "CreateAndGetUserGameSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true, createNewUser: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.AreEqual(1, matches);
		}

		[Test]
		public void CreateAndGetUserGlobalSaveData()
		{
			var userDataName = "CreateAndGetUserGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewUser: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == null && g.ActorId == newSaveData.ActorId);

			Assert.AreEqual(1, matches);
		}

		[Test]
		public void CreateAndGetGameGlobalSaveData()
		{
			var userDataName = "CreateAndGetGameGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == newSaveData.Key && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.AreEqual(1, matches);
		}

		[Test]
		public void GetMultipleUserSaveDatas()
		{
			var userDataNames = new[]
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

			var userDatas = _gameDataController.Get(gameId, userId, userDataNames);

			var matchingUserSaveDatas = userDatas.Select(g => userDataNames.Contains(g.Key));

			Assert.AreEqual(matchingUserSaveDatas.Count(), userDataNames.Length);
		}

		[Test]
		public void GetUserSaveDatasWithNonExistingKey()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingKey";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { "null key" });

			Assert.IsEmpty(userDatas);
		}

		[Test]
		public void GetUserSaveDatasWithNonExistingGame()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingGame";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(-1, newSaveData.ActorId, new string[] { userDataName });

			Assert.IsEmpty(userDatas);
		}

		[Test]
		public void GetUserSaveDatasWithNonExistingUser()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingUser";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(newSaveData.GameId, -1, new string[] { userDataName });

			Assert.IsEmpty(userDatas);
		}
		#endregion

		#region Helpers
		private Model.GameData CreateGameData(string key, int? gameId = null, int? userId = null, 
			bool createNewGame = false, bool createNewUser = false)
		{
			if (createNewGame)
			{
				var game = new Game
				{
					Name = key
				};
				_gameController.Create(game);
				gameId = game.Id;
			}

			if (createNewUser)
			{
				var user = new User
				{
					Name = key
				};
				_userController.Create(user);
				userId = user.Id;
			}

			var userData = new Model.GameData
			{
				Key = key,
				GameId = gameId,
				ActorId = userId,
				Value = key + " value",
				DataType = 0
			};
			_gameDataController.Create(userData);
			return userData;
		}
		#endregion
	}
}
