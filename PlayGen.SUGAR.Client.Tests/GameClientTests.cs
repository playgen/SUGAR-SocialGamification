using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameClientTests : ClientTestsBase
	{
		[Test]
		public void CanCreateGame()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanCreateGame",
			};

			var response = SUGARClient.Game.Create(gameRequest);

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

			SUGARClient.Game.Create(gameRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Create(gameRequest));
		}

		[Test]
		public void CannotCreateGameWithNoName()
		{
			var gameRequest = new GameRequest{};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Create(gameRequest));
		}

		[Test]
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

			Assert.AreEqual(2, getGames.Count());
		}

		[Test]
		public void CannotGetNotExistingGameByName()
		{
			var getGames = SUGARClient.Game.Get("CannotGetNotExistingGameByName");

			Assert.IsEmpty(getGames);
		}

		[Test]
		public void CannotGetGameByEmptyName()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Game.Get(""));
		}

		[Test]
		public void CanGetGameById()
		{
			var gameRequest = new GameRequest
			{
				Name = "CanGetGameById",
			};

			var response = SUGARClient.Game.Create(gameRequest);

			var getGame = SUGARClient.Game.Get((int) response.Id);

			Assert.AreEqual(response.Name, getGame.Name);
			Assert.AreEqual(gameRequest.Name, getGame.Name);
		}

		[Test]
		public void CannotGetNotExistingGameById()
		{
			var getGame = SUGARClient.Game.Get(-1);

			Assert.Null(getGame);
		}

		[Test]
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

		[Test]
		public void CannotUpdateNonExistingGame()
		{
			var updateGame = new GameRequest
			{
				Name = "CannotUpdateNonExistingGame"
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Game.Update(-1, updateGame));
		}

		[Test]
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

		[Test]
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

		[Test]
		public void CannotDeleteNonExistingGame()
		{
			SUGARClient.Game.Delete(-1);
		}
	}
}