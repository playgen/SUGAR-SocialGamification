using System.Diagnostics.Eventing.Reader;
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

		// TODO test the rest of the game controller fucntionaity
		#endregion
	}
}