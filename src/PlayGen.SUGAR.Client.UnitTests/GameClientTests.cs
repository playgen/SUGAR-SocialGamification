using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
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
		[Fact]
		public void CanCreateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanCreateGame",
			};

			var response = _gameClient.Create(gameRequest);

			Assert.Equal(gameRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Fact]
		public void CannotCreateDuplicateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CannotCreateDuplicateGame",
			};

			_gameClient.Create(gameRequest);

			Assert.Throws<WebException>(() => _gameClient.Create(gameRequest));
		}

		[Fact]
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

			Assert.Equal(2, getGames.Count());
		}

		[Fact]
		public void CannotGetNotExistingGameByName()
		{
			var getGames = _gameClient.Get("CannotGetNotExistingGameByName");

			Assert.Empty(getGames);
		}

		[Fact]
		public void CanGetGameById()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanGetGameById",
			};

			var response = _gameClient.Create(gameRequest);

			var getGame = _gameClient.Get(response.Id);

			Assert.Equal(response.Name, getGame.Name);
			Assert.Equal(gameRequest.Name, getGame.Name);
		}

		[Fact]
		public void CannotGetNotExistingGameById()
		{
			var getGame = _gameClient.Get(-1);

			Assert.Null(getGame);
		}

		[Fact]
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

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal("CanUpdateGame Updated", updateRequest.Name);
		}

		[Fact]
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

			Assert.Throws<WebException>(() => _gameClient.Update(responseTwo.Id, updateGame));
		}

		[Fact]
		public void CannotUpdateNonExistingGame()
		{
			var updateGame = new GameRequest
			{
				Name = "CannotUpdateNonExistingGame"
			};

			Assert.Throws<WebException>(() => _gameClient.Update(-1, updateGame));
		}

		// TODO test the rest of the game controller fucntionaity
		#endregion
	}
}