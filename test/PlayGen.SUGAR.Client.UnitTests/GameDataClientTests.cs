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
		[Test]
		public void CanCreate()
		{
			var user = GetOrCreateUser("Create");
			var game = GetOrCreateGame("Create");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.AreEqual(gameDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(gameDataRequest.GameId, response.GameId);
			Assert.AreEqual(gameDataRequest.Key, response.Key);
			Assert.AreEqual(gameDataRequest.Value, response.Value);
			Assert.AreEqual(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = GetOrCreateUser("Create");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.AreEqual(gameDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(gameDataRequest.GameId, response.GameId);
			Assert.AreEqual(gameDataRequest.Key, response.Key);
			Assert.AreEqual(gameDataRequest.Value, response.Value);
			Assert.AreEqual(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = GetOrCreateGame("Create");

			var gameDataRequest = new GameDataRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.AreEqual(gameDataRequest.ActorId, response.ActorId);
			Assert.AreEqual(gameDataRequest.GameId, response.GameId);
			Assert.AreEqual(gameDataRequest.Key, response.Key);
			Assert.AreEqual(gameDataRequest.Value, response.Value);
			Assert.AreEqual(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Test]
		public void CannotCreateWithoutKey()
		{
			var user = GetOrCreateUser("Create");
			var game = GetOrCreateGame("Create");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Test]
		public void CannotCreateWithoutValue()
		{
			var user = GetOrCreateUser("Create");
			var game = GetOrCreateGame("Create");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithoutKey",
				GameDataType = GameDataType.String,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Test]
		public void CannotCreateWithMismatchedData()
		{
			var user = GetOrCreateUser("Create");
			var game = GetOrCreateGame("Create");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				GameDataType = GameDataType.Float,
			};

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Long;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Boolean;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Float;
			gameDataRequest.Value = "True";

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Long;

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));

			gameDataRequest.GameDataType = GameDataType.Boolean;
			gameDataRequest.Value = "2";

			Assert.Throws<ClientException>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Test]
		public void CanGetGameData()
		{
			var user = GetOrCreateUser("Get");
			var game = GetOrCreateGame("Get");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameData",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(user.Id, game.Id, new string[] { "CanGetGameData" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().GameDataType, response.GameDataType);
		}

		[Test]
		public void CanGetGameDataWithoutActorId()
		{
			var game = GetOrCreateGame("Get");

			var gameDataRequest = new GameDataRequest
			{
				GameId = game.Id,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(null, game.Id, new string[] { "CanGetGameDataWithoutActorId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().GameDataType, response.GameDataType);
		}

		[Test]
		public void CanGetGameDataWithoutGameId()
		{
			var user = GetOrCreateUser("Get");

			var gameDataRequest = new GameDataRequest
			{
				ActorId = user.Id,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			var get = _gameDataClient.Get(user.Id, null, new string[] { "CanGetGameDataWithoutGameId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(get.First().ActorId, response.ActorId);
			Assert.AreEqual(get.First().GameId, response.GameId);
			Assert.AreEqual(get.First().Key, response.Key);
			Assert.AreEqual(get.First().Value, response.Value);
			Assert.AreEqual(get.First().GameDataType, response.GameDataType);
		}

		[Test]
		public void CanGetGameDataByMultipleKeys()
		{
			var user = GetOrCreateUser("Get");
			var game = GetOrCreateGame("Get");

			var gameDataRequestOne = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var gameDataRequestTwo = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var gameDataRequestThree = new GameDataRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var responseOne = _gameDataClient.Add(gameDataRequestOne);
			var responseTwo = _gameDataClient.Add(gameDataRequestTwo);
			var responseThree = _gameDataClient.Add(gameDataRequestThree);

			var get = _gameDataClient.Get(user.Id, game.Id, new string[] { "CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3" });

			Assert.AreEqual(3, get.Count());
			foreach (var g in get)
			{
				Assert.AreEqual("Test Value", g.Value);
			}
		}
		#endregion
		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "GameDataControllerTests" + suffix ?? $"_{suffix}";
			var users = _userClient.Get(name, true);
			ActorResponse user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = _userClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return user;
		}

		private GameResponse GetOrCreateGame(string suffix)
		{
			string name = "GameDataControllerTests" + suffix ?? $"_{suffix}";
			var games = _gameClient.Get(name);
			GameResponse game;

			if (games.Any())
			{
				game = games.Single();
			}
			else
			{
				game = _gameClient.Create(new GameRequest
				{
					Name = name
				});
			}

			return game;
		}
		#endregion
	}
}