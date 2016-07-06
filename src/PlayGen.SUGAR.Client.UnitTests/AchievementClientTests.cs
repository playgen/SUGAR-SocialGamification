using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class AchievementClientTests
	{
		#region Configuration
		private readonly AchievementClient _achievementClient;

		public AchievementClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_achievementClient = testSugarClient.Achievement;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "AchievementClientTests",
				Password = "AchievementClientTestsPassword",
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
		public void CanCreateAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanCreateAchievement",
				ActorType = ActorType.User,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanCreateAchievement",
						ComparisonType = ComparisonType.Equals,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
				Token = "CanCreateAchiement"
			};

			var response = _achievementClient.Create(achievementRequest);

			Assert.Equal(achievementRequest.Token, response.Token);
			Assert.True(response.Id > 0);
		}

		//[Fact]
		//public void CannotCreateDuplicateGame()
		//{
		//	var gameRequest = new GameRequest
		//	{
		//		Name = "CannotCreateDuplicateGame",
		//	};

		//	_achievementClient.Create(gameRequest);

		//	Assert.Throws<WebException>(() => _achievementClient.Create(gameRequest));
		//}

		// TODO test the rest of the game controller fucntionaity
		#endregion
	}
}
