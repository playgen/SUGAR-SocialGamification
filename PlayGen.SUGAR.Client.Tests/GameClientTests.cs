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
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGames = Fixture.SUGARClient.Game.Get(key);

			Assert.Equal(2, getGames.Count());
		}

		[Fact]
		public void CannotGetNotExistingGameByName()
		{
			var key = "Game_CannotGetNotExistingGameByName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGames = Fixture.SUGARClient.Game.Get(key);

			Assert.Empty(getGames);
		}

		[Fact]
		public void CannotGetGameByEmptyName()
		{
			var key = "Game_CannotGetGameByEmptyName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Game.Get(string.Empty));
		}

		[Fact]
		public void CanGetGameById()
		{
			var key = "Game_CanGetGameById";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			var getGame = Fixture.SUGARClient.Game.Get(game.Id);

			Assert.Equal(key, getGame.Name);
			Assert.Equal(game.Name, getGame.Name);
		}

		[Fact]
		public void CannotGetNotExistingGameById()
		{
			var key = "Game_CannotGetNotExistingGameById";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGame = Fixture.SUGARClient.Game.Get(-1);

			Assert.Null(getGame);
		}

		public GameClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}