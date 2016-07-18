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
		private readonly GameClient _gameClient;

		public AchievementClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_achievementClient = testSugarClient.Achievement;
			_gameClient = testSugarClient.Game;

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
				Token = "CanCreateAchievement",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanCreateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			Assert.Equal(achievementRequest.Token, response.Token);
			Assert.Equal(achievementRequest.ActorType, response.ActorType);
		}

		public void CanCreateGlobalAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanCreateGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanCreateGlobalAchievement",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanCreateGlobalAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			Assert.Equal(achievementRequest.Token, response.Token);
			Assert.Equal(achievementRequest.ActorType, response.ActorType);
		}

		[Fact]
		public void CannotCreateDuplicateAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateDuplicateAchievement",
				ActorType = ActorType.User,
				Token = "CannotCreateDuplicateAchievement",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateDuplicateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			_achievementClient.Create(achievementRequest);

			Assert.Throws<WebException>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CanGetAchievementByKeys()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanGetAchievementByKeys",
				ActorType = ActorType.User,
				Token = "CanGetAchievementByKeys",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetAchievementByKeys",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var getAchievement = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.Equal(response.Name, getAchievement.Name);
			Assert.Equal(achievementRequest.Name, getAchievement.Name);
		}

		[Fact]
		public void CannotGetNotExistingAchievementByKeys()
		{
			var getAchievement = _achievementClient.GetById("CannotGetNotExistingAchievementByKeys", 1);

			Assert.Null(getAchievement);
		}

		[Fact]
		public void CanGetGlobalAchievementByToken()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanGetGlobalAchievementByToken",
				ActorType = ActorType.User,
				Token = "CanGetGlobalAchievementByToken",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetGlobalAchievementByToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var getAchievement = _achievementClient.GetGlobalById(achievementRequest.Token);

			Assert.Equal(response.Name, getAchievement.Name);
			Assert.Equal(achievementRequest.Name, getAchievement.Name);
		}

		[Fact]
		public void CannotGetNotExistingGlobalAchievementByKeys()
		{
			var getAchievement = _achievementClient.GetGlobalById("CannotGetNotExistingGlobalAchievementByKeys");

			Assert.Null(getAchievement);
		}

		// TODO test the rest of the game controller fucntionaity
		#endregion
	}
}
