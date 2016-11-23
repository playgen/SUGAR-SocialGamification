using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GameDataClientTests
	{
		#region Configuration
		private readonly GameDataClient _gameDataClient;
		private readonly UserClient _userClient;
		private readonly GameClient _gameClient;

		public GameDataClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_gameDataClient = testSugarClient.GameData;
			_userClient = testSugarClient.User;
			_gameClient = testSugarClient.Game;

			Helpers.RegisterAndLogin(testSugarClient.Account);
		}
		#endregion

		#region Tests
		[Test]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Create");
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			Assert.AreEqual(SaveDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(SaveDataRequest.GameId, response.GameId);
			Assert.AreEqual(SaveDataRequest.Key, response.Key);
			Assert.AreEqual(SaveDataRequest.Value, response.Value);
			Assert.AreEqual(SaveDataRequest.SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CannotCreateWithoutKey()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Create");
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));
		}

		[Test]
		public void CannotCreateWithoutValue()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Create");
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithoutKey",
				SaveDataType = SaveDataType.String,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));
		}

		[Test]
		public void CannotCreateWithMismatchedData()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Create");
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				SaveDataType = SaveDataType.Float,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Long;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Boolean;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Float;
			SaveDataRequest.Value = "True";

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Long;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));

			SaveDataRequest.SaveDataType = SaveDataType.Boolean;
			SaveDataRequest.Value = "2";

			Assert.Throws<ClientException>(() => _gameDataClient.Add(SaveDataRequest));
		}

		[Test]
		public void CanGetGameData()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Get");
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameData",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			var get = _gameDataClient.Get(user.Id, game.Id, new string[] { "CanGetGameData" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var SaveDataRequest = new SaveDataRequest
			{
				GameId = game.Id,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			var get = _gameDataClient.Get(null, game.Id, new string[] { "CanGetGameDataWithoutActorId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Get");

			var SaveDataRequest = new SaveDataRequest
			{
				ActorId = user.Id,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var response = _gameDataClient.Add(SaveDataRequest);

			var get = _gameDataClient.Get(user.Id, null, new string[] { "CanGetGameDataWithoutGameId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().SaveDataType, response.SaveDataType);
		}

		[Test]
		public void CanGetGameDataByMultipleKeys()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "Get");
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var SaveDataRequestOne = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var SaveDataRequestTwo = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var SaveDataRequestThree = new SaveDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				SaveDataType = SaveDataType.String,
			};

			var responseOne = _gameDataClient.Add(SaveDataRequestOne);
			var responseTwo = _gameDataClient.Add(SaveDataRequestTwo);
			var responseThree = _gameDataClient.Add(SaveDataRequestThree);

			var get = _gameDataClient.Get(user.Id, game.Id, new string[] { "CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3" });

			Assert.AreEqual(3, get.Count());
			foreach (var g in get)
			{
				Assert.AreEqual("Test Value", g.Value);
			}
		}
		#endregion
	}
}