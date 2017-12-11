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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGames = Fixture.SUGARClient.Game.Get(key);

			Assert.Equal(2, getGames.Count());
		}

		[Fact]
		public void CannotGetNotExistingGameByName()
		{
			var key = "Game_CannotGetNotExistingGameByName";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGames = Fixture.SUGARClient.Game.Get(key);

			Assert.Empty(getGames);
		}

		[Fact]
		public void CannotGetGameByEmptyName()
		{
			var key = "Game_CannotGetGameByEmptyName";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Game.Get(string.Empty));
		}

		[Fact]
		public void CanGetGameById()
		{
			var key = "Game_CanGetGameById";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var getGame = Fixture.SUGARClient.Game.Get(game.Id);

			Assert.Equal(key, getGame.Name);
			Assert.Equal(game.Name, getGame.Name);
		}

		[Fact]
		public void CannotGetNotExistingGameById()
		{
			var key = "Game_CannotGetNotExistingGameById";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGame = Fixture.SUGARClient.Game.Get(-1);

			Assert.Null(getGame);
		}

		public GameClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}