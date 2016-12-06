using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	[Collection("Project Fixture Collection")]
	public class GameDataControllerTests
	{
		#region Configuration
		private readonly GameDataController _gameDataController = ControllerLocator.GameDataController;
		private readonly GameController _gameController = ControllerLocator.GameController;
		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion
		
		#region Tests
		[Fact]
		public void CreateAndGetUserGameSaveData()
		{
			var userDataName = "CreateAndGetUserGameSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true, createNewUser: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new List<string> { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetUserGlobalSaveData()
		{
			var userDataName = "CreateAndGetUserGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewUser: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new List<string> { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == null && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetGameGlobalSaveData()
		{
			var userDataName = "CreateAndGetGameGlobalSaveData";

			var newSaveData = CreateGameData(userDataName, createNewGame: true);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new List<string> { newSaveData.Key });

			var matches = userDatas.Count(g => g.Key == newSaveData.Key && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void GetMultipleUserSaveDatas()
		{
			var userDataNames = new List<string>
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

			Assert.Equal(matchingUserSaveDatas.Count(), userDataNames.Count);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingKey()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingKey";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new List<string> { "null key" });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingGame()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingGame";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(-1, newSaveData.ActorId, new List<string> { userDataName });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserSaveDatasWithNonExistingUser()
		{
			var userDataName = "GetUserSaveDatasWithNonExistingUser";

			var newSaveData = CreateGameData(userDataName);

			var userDatas = _gameDataController.Get(newSaveData.GameId, -1, new List<string> { userDataName });

			Assert.Empty(userDatas);
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
				SaveDataType = 0
			};
			_gameDataController.Create(userData);
			return userData;
		}
		#endregion
	}
}
