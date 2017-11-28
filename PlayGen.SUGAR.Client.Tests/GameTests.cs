using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameTests : ClientTestBase
	{
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
	}
}