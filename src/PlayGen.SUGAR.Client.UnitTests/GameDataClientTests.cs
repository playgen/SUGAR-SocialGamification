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
		[Fact]
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

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
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

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
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

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}

		[Fact]
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

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Fact]
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

			Assert.Throws<Exception>(() => _gameDataClient.Add(gameDataRequest));
		}

		[Fact]
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

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().ActorId, response.ActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().GameDataType, response.GameDataType);
		}

		[Fact]
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

			Assert.Equal(3, get.Count());
			foreach (var g in get)
			{
				Assert.Equal("Test Value", g.Value);
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