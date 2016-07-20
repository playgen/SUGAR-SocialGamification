using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;
using System.Linq;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class GameDataClientTests
	{
		#region Configuration
		private readonly GameDataClient _gameDataClient;

		public GameDataClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_gameDataClient = testSugarClient.GameData;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "GameDataClientTests",
				Password = "GameDataClientTestsPassword",
				AutoLogin = true,
			};

			try
			{
				client.Login(accountRequest);
			}
			catch
			{
				client.Register(accountRequest);
			}
		}
		#endregion

		#region Tests
		[Fact]
		public void CanCreate()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CanCreate",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
		public void CanCreateWithoutGameId()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
		public void CanCreateWithoutActorId()
		{
			var gameDataRequest = new GameDataRequest
			{
				GameId = 1,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
		public void CannotCreateWithoutKey()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutValue()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CannotCreateWithoutKey",
				GameDataType = GameDataType.String,
			};

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Fact]
		public void CannotCreateWithMismatchedData()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				GameDataType = GameDataType.Float,
			};

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Long;

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Boolean;

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Float;
			gameDataRequest.Value = "True";

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Long;

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Boolean;
			gameDataRequest.Value = "2";

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Fact]
		public void CanGetGameData()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CanGetGameData",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(1, 1, new string[] { "CanGetGameData" });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().ActorId, response.ActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().GameDataType, response.GameDataType);
		}

		[Fact]
		public void CanGetGameDataWithoutActorId()
		{
			var gameDataRequest = new GameDataRequest
			{
				GameId = 1,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(null, 1, new string[] { "CanGetGameDataWithoutActorId" });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().ActorId, response.ActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().GameDataType, response.GameDataType);
		}

		[Fact]
		public void CanGetGameDataWithoutGameId()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = 1,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(1, null, new string[] { "CanGetGameDataWithoutGameId" });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().ActorId, response.ActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().GameDataType, response.GameDataType);
		}

		[Fact]
		public void CanGetGameDatByMultipleKeys()
		{
			var gameDataRequestOne = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var gameDataRequestTwo = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var gameDataRequestThree = new GameDataRequest
			{
				ActorId = 1,
				GameId = 1,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var responseOne = _gameDataClient.Add(gameDataRequestOne);
			var responseTwo = _gameDataClient.Add(gameDataRequestTwo);
			var responseThree = _gameDataClient.Add(gameDataRequestThree);

			var get = _gameDataClient.Get(1, 1, new string[] { "CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3" });

			Assert.Equal(3, get.Count());
		}
		#endregion
	}
}