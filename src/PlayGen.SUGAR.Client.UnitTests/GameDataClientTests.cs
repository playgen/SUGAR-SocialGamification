using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;
using System.Linq;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class GameDataClientTests
	{
		#region Configuration
		private readonly GameDataClient _gameDataClient;

		public GameDataClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_gameDataClient = testSugarClient.GameData;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "GameDataClientTests",
				Password = "GameDataClientTestsPassword",
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
		public void CanCreate()
		{
			var gameDataRequest = new GameDataRequest
			{
				ActorId = null,
				GameId = null,
				Key = "CanCreate",
				Value = "Test Value",
				GameDataType = GameDataType.String,
			};

			var response = _gameDataClient.Add(gameDataRequest);

			Assert.Equal(gameDataRequest.ActorId, response.ActorId);
			Assert.Equal(gameDataRequest.GameId, response.GameId);
			Assert.Equal(gameDataRequest.Key, response.Key);
			Assert.Equal(gameDataRequest.Value, response.Value);
			Assert.Equal(gameDataRequest.GameDataType, response.GameDataType);
		}
		#endregion
	}
}