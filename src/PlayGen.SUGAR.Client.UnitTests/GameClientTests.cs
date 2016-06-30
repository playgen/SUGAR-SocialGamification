using PlayGen.SUGAR.Contracts;
using Xunit;

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

			var response = _gameClient.Create(gameRequest);

			// TODO make sure the second create game failed in order for this test to pass
		}
		#endregion
	}
}