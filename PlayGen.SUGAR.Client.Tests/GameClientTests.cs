using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameClientTests : ClientTestBase
	{
		[Fact]
		public void CanGetGamesByName()
		{
			var key = "Game_CanGetGamesByName";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGames = SUGARClient.Game.Get(key);

			Assert.Equal(2, getGames.Count());
		}

		[Fact]
		public void CannotGetNotExistingGameByName()
		{
			var key = "Game_CannotGetNotExistingGameByName";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGames = SUGARClient.Game.Get(key);

			Assert.Empty(getGames);
		}

		[Fact]
		public void CannotGetGameByEmptyName()
		{
			var key = "Game_CannotGetGameByEmptyName";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			Assert.Throws<ClientException>(() => SUGARClient.Game.Get(string.Empty));
		}

		[Fact]
		public void CanGetGameById()
		{
			var key = "Game_CanGetGameById";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var getGame = SUGARClient.Game.Get(game.Id);

			Assert.Equal(key, getGame.Name);
			Assert.Equal(game.Name, getGame.Name);
		}

		[Fact]
		public void CannotGetNotExistingGameById()
		{
			var key = "Game_CannotGetNotExistingGameById";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGame = SUGARClient.Game.Get(-1);

			Assert.Null(getGame);
		}
	}
}