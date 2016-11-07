using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GameClientTests
	{
		#region Configuration
		private readonly GameClient _gameClient;

		public GameClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_gameClient = testSugarClient.Game;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "GameClientTests",
				Password = "GameClientTestsPassword",
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
		public void CanCreateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanCreateGame",
			};

			var response = _gameClient.Create(gameRequest);

			Assert.AreEqual(gameRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Test]
		public void CannotCreateDuplicateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CannotCreateDuplicateGame",
			};

			_gameClient.Create(gameRequest);

			Assert.Throws<ClientException>(() => _gameClient.Create(gameRequest));
		}

		[Test]
		public void CannotCreateGameWithNoName()
		{
			var gameRequest = new GameRequest{};

			Assert.Throws<ClientException>(() => _gameClient.Create(gameRequest));
		}

		[Test]
		public void CanGetGamesByName()
		{
			var gameRequestOne = new GameRequest
			{
				Name = "CanGetGamesByName 1",
			};

			var responseOne = _gameClient.Create(gameRequestOne);

			var gameRequestTwo = new GameRequest
			{
				Name = "CanGetGamesByName 2",
			};

			var responseTwo = _gameClient.Create(gameRequestTwo);

			var getGames = _gameClient.Get("CanGetGamesByName");

			Assert.AreEqual(2, getGames.Count());
		}

		[Test]
		public void CannotGetNotExistingGameByName()
		{
			var getGames = _gameClient.Get("CannotGetNotExistingGameByName");

			Assert.IsEmpty(getGames);
		}

		[Test]
		public void CannotGetGameByEmptyName()
		{
			Assert.Throws<ClientException>(() => _gameClient.Get(""));
		}

		[Test]
		public void CanGetGameById()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanGetGameById",
			};

			var response = _gameClient.Create(gameRequest);

			var getGame = _gameClient.Get(response.Id);

			Assert.AreEqual(response.Name, getGame.Name);
			Assert.AreEqual(gameRequest.Name, getGame.Name);
		}

		[Test]
		public void CannotGetNotExistingGameById()
		{
			var getGame = _gameClient.Get(-1);

			Assert.Null(getGame);
		}

		[Test]
		public void CanUpdateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanUpdateGame",
			};

			var response = _gameClient.Create(gameRequest);

			var updateRequest = new GameRequest
			{
				Name = "CanUpdateGame Updated"
			};

			_gameClient.Update(response.Id, updateRequest);

			var getGame = _gameClient.Get(response.Id);

			Assert.AreNotEqual(response.Name, updateRequest.Name);
			Assert.AreEqual("CanUpdateGame Updated", getGame.Name);
		}

		[Test]
		public void CannotUpdateGameToDuplicateName()
		{
			var gameRequestOne = new GameRequest
			{
				Name = "CannotUpdateGameToDuplicateName 1"
			};

			var responseOne = _gameClient.Create(gameRequestOne);

			var gameRequestTwo = new GameRequest
			{
				Name = "CannotUpdateGameToDuplicateName 2"
			};

			var responseTwo = _gameClient.Create(gameRequestTwo);

			var updateGame = new GameRequest
			{
				Name = gameRequestOne.Name
			};

			Assert.Throws<ClientException>(() => _gameClient.Update(responseTwo.Id, updateGame));
		}

		[Test]
		public void CannotUpdateNonExistingGame()
		{
			var updateGame = new GameRequest
			{
				Name = "CannotUpdateNonExistingGame"
			};

			Assert.Throws<ClientException>(() => _gameClient.Update(-1, updateGame));
		}

		[Test]
		public void CannotUpdateGameToNoName()
		{
			var gameRequest = new GameRequest
			{
				Name = "CannotUpdateGameToNoName",
			};

			var response = _gameClient.Create(gameRequest);

			var updateRequest = new GameRequest
			{
			};

			Assert.Throws<ClientException>(() => _gameClient.Update(response.Id, updateRequest));
		}

		[Test]
		public void CanDeleteGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanDeleteGame",
			};

			var response = _gameClient.Create(gameRequest);

			var getGame = _gameClient.Get(response.Id);

			Assert.NotNull(getGame);

			_gameClient.Delete(response.Id);

			getGame = _gameClient.Get(response.Id);

			Assert.Null(getGame);
		}

		[Test]
		public void CannotDeleteNonExistingGame()
		{
			_gameClient.Delete(-1);
		}


		#endregion
	}
}