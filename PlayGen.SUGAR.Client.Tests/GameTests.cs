using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameTests : ClientTestBase
	{
		[Fact]
		public void CanCreateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanCreateGame",
			};

			var response = SUGARClient.Game.Create(gameRequest);

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

			SUGARClient.Game.Create(gameRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Create(gameRequest));
		}

		[Fact]
		public void CannotCreateGameWithNoName()
		{
			var gameRequest = new GameRequest{};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Create(gameRequest));
		}

		[Fact]
		public void CanGetGamesByName()
		{
			var gameRequestOne = new GameRequest
			{
				Name = "CanGetGamesByName 1",
			};

			var responseOne = SUGARClient.Game.Create(gameRequestOne);

			var gameRequestTwo = new GameRequest
			{
				Name = "CanGetGamesByName 2",
			};

			var responseTwo = SUGARClient.Game.Create(gameRequestTwo);

			var getGames = SUGARClient.Game.Get("CanGetGamesByName");

			Assert.Equal(2, getGames.Count());
		}

		[Fact]
		public void CannotGetNotExistingGameByName()
		{
			var getGames = SUGARClient.Game.Get("CannotGetNotExistingGameByName");

			Assert.Empty(getGames);
		}

		[Fact]
		public void CannotGetGameByEmptyName()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Game.Get(""));
		}

		[Fact]
		public void CanGetGameById()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanGetGameById",
			};

			var response = SUGARClient.Game.Create(gameRequest);

			var getGame = SUGARClient.Game.Get((int) response.Id);

			Assert.Equal(response.Name, getGame.Name);
			Assert.Equal(gameRequest.Name, getGame.Name);
		}

		[Fact]
		public void CannotGetNotExistingGameById()
		{
			var getGame = SUGARClient.Game.Get(-1);

			Assert.Null(getGame);
		}

		[Fact]
		public void CanUpdateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanUpdateGame",
			};

			var response = SUGARClient.Game.Create(gameRequest);

			var updateRequest = new GameRequest
			{
				Name = "CanUpdateGame Updated"
			};

			SUGARClient.Game.Update(response.Id, updateRequest);

			var getGame = SUGARClient.Game.Get((int) response.Id);

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal("CanUpdateGame Updated", getGame.Name);
		}

		[Fact]
		public void CannotUpdateGameToDuplicateName()
		{
			var gameRequestOne = new GameRequest
			{
				Name = "CannotUpdateGameToDuplicateName 1"
			};

			var responseOne = SUGARClient.Game.Create(gameRequestOne);

			var gameRequestTwo = new GameRequest
			{
				Name = "CannotUpdateGameToDuplicateName 2"
			};

			var responseTwo = SUGARClient.Game.Create(gameRequestTwo);

			var updateGame = new GameRequest
			{
				Name = gameRequestOne.Name
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Update(responseTwo.Id, updateGame));
		}

		[Fact]
		public void CannotUpdateNonExistingGame()
		{
			var updateGame = new GameRequest
			{
				Name = "CannotUpdateNonExistingGame"
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Update(-1, updateGame));
		}

		[Fact]
		public void CannotUpdateGameToNoName()
		{
			var gameRequest = new GameRequest
			{
				Name = "CannotUpdateGameToNoName",
			};

			var response = SUGARClient.Game.Create(gameRequest);

			var updateRequest = new GameRequest
			{
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Update(response.Id, updateRequest));
		}

		[Fact]
		public void CanDeleteGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanDeleteGame",
			};

			var response = SUGARClient.Game.Create(gameRequest);

			var getGame = SUGARClient.Game.Get((int) response.Id);

			Assert.NotNull(getGame);

			SUGARClient.Game.Delete(response.Id);

			getGame = SUGARClient.Game.Get((int) response.Id);

			Assert.Null(getGame);
		}

		[Fact]
		public void CannotDeleteNonExistingGame()
		{
			SUGARClient.Game.Delete(-1);
		}
	}
}