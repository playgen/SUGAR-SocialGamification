using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class AchievementClientTests
	{
		#region Configuration
		private readonly AchievementClient _achievementClient;
		private readonly GameDataClient _gameDataClient;

		public AchievementClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_achievementClient = testSugarClient.Achievement;
			_gameDataClient = testSugarClient.GameData;

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

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoName()
		{
			var achievementRequest = new AchievementRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoName",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateAchievementWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoToken()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoCompletionCriteria()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateAchievementWithNoCompletionCriteria",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoCompletionCriteria",
				GameId = 1,
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoAchievementCriteriaKey()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateAchievementWithNoAchievementCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoAchievementCriteriaKey",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoAchievementCriteriaValue()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateAchievementWithNoAchievementCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoAchievementCriteriaValue",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateAchievementWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
		}

		[Fact]
		public void CannotCreateAchievementWithNoAchievementCriteriaDataTypeMismatch()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotCreateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateAchievementWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Create(achievementRequest));
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
		public void CannotGetAchievementByEmptyToken()
		{
			Assert.Throws<Exception>(() => _achievementClient.GetById("", 1));
		}

		[Fact]
		public void CanGetAchievementByKeysThatContainSlashes()
		{
			var getAchievement = _achievementClient.GetById("Can/Get/Achievement/By/Keys/That/Contain/Slashes", 1);

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

		[Fact]
		public void CannotGetGlobalAchievementByEmptyToken()
		{
			Assert.Throws<Exception>(() => _achievementClient.GetGlobalById(""));
		}

		[Fact]
		public void CanGetGlobalAchievementByKeysThatContainSlashes()
		{
			var getAchievement = _achievementClient.GetGlobalById("Can/Get/Achievement/By/Keys/That/Contain/Slashes");

			Assert.Null(getAchievement);
		}

		[Fact]
		public void CannotGetByAchievementsByNotExistingGameId()
		{
			var getAchievements = _achievementClient.GetByGame(-1);

			Assert.Empty(getAchievements);
		}

		[Fact]
		public void CanUpdateAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanUpdateAchievement",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanUpdateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CanUpdateAchievement Updated",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanUpdateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			_achievementClient.Update(updateRequest);

			var updateResponse = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.NotEqual(response.Name, updateResponse.Name);
			Assert.Equal("CanUpdateAchievement Updated", updateResponse.Name);
		}

		[Fact]
		public void CannotUpdateAchievementToDuplicateName()
		{
			var achievementRequestOne = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameOne",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementToDuplicateNameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = _achievementClient.Create(achievementRequestOne);

			var achievementRequestTwo = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementToDuplicateNameTwo",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameTwo",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = _achievementClient.Create(achievementRequestTwo);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameTwo",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateNonExistingAchievement()
		{
			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateNonExistingAchievement",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateNonExistingAchievement",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateNonExistingAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievemenWithNoName()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievemenWithNoName",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievementWithNoToken()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoToken",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoToken",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievementWithNoCompletionCriteria()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoCompletionCriteria",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoCompletionCriteria",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoCompletionCriteria",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoCompletionCriteria",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoCompletionCriteria",
				GameId = 1,
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievementWithNoAchievementCriteriaKey()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaKey",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaKey",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoAchievementCriteriaKey",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaKey",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievementWithNoAchievementCriteriaValue()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				GameId = 1,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateAchievementWithNoAchievementCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<Exception>(() => _achievementClient.Update(updateRequest));
		}

		[Fact]
		public void CanDeleteAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanDeleteAchievement",
				ActorType = ActorType.User,
				Token = "CanDeleteAchievement",
				GameId = 1,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanDeleteAchievement",
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

			Assert.NotNull(getAchievement);

			_achievementClient.Delete(achievementRequest.Token, achievementRequest.GameId.Value);

			getAchievement = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.Null(getAchievement);
		}

		[Fact]
		public void CannotDeleteNonExistingAchievement()
		{
			_achievementClient.Delete("CannotDeleteNonExistingAchievement", 1);
		}

		[Fact]
		public void CannotDeleteAchievementByEmptyToken()
		{
			Assert.Throws<Exception>(() => _achievementClient.Delete("", 1));
		}

		[Fact]
		public void CanDeleteGlobalAchievement()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanDeleteGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanDeleteGlobalAchievement",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanDeleteGlobalAchievement",
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

			Assert.NotNull(getAchievement);

			_achievementClient.DeleteGlobal(achievementRequest.Token);

			getAchievement = _achievementClient.GetGlobalById(achievementRequest.Token);

			Assert.Null(getAchievement);
		}

		[Fact]
		public void CannotDeleteNonExistingGlobalAchievement()
		{
			_achievementClient.DeleteGlobal("CannotDeleteNonExistingGlobalAchievement");
		}

		[Fact]
		public void CannotDeleteGlobalAchievementByEmptyToken()
		{
			Assert.Throws<Exception>(() => _achievementClient.DeleteGlobal(""));
		}

		[Fact]
		public void CanGetGlobalAchievementProgress()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanGetGlobalAchievementProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalAchievementProgress",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetGlobalAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var progressGame = _achievementClient.GetGlobalProgress(1);
			Assert.True(progressGame.Count() > 0);

			var progressAchievement = _achievementClient.GetGlobalAchievementProgress(response.Token, 1);
			Assert.Equal(0, progressAchievement.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetGlobalAchievementProgress",
				Value = "1",
				ActorId = 1,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressAchievement = _achievementClient.GetGlobalAchievementProgress(response.Token, 1);
			Assert.Equal(1, progressAchievement.Progress);
		}

		[Fact]
		public void CannotGetNotExistingGlobalAchievementProgress()
		{
			Assert.Throws<Exception>(() => _achievementClient.GetGlobalAchievementProgress("CannotGetNotExistingGlobalAchievementProgress", 1));
		}

		[Fact]
		public void CanGetAchievementProgress()
		{
			var achievementRequest = new AchievementRequest()
			{
				Name = "CanGetAchievementProgress",
				GameId = 10,
				ActorType = ActorType.Undefined,
				Token = "CanGetAchievementProgress",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var progressGame = _achievementClient.GetGameProgress(10, 1);
			Assert.True(progressGame.Count() > 0);

			var progressAchievement = _achievementClient.GetAchievementProgress(response.Token, 10, 1);
			Assert.Equal(0, progressAchievement.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetAchievementProgress",
				Value = "1",
				ActorId = 1,
				GameId = 10,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressAchievement = _achievementClient.GetAchievementProgress(response.Token, 10, 1);
			Assert.Equal(1, progressAchievement.Progress);
		}

		[Fact]
		public void CannotGetNotExistingAchievementProgress()
		{
			Assert.Throws<Exception>(() => _achievementClient.GetAchievementProgress("CannotGetNotExistingAchievementProgress", 10, 1));
		}
		#endregion
	}
}
