using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class SkillClientTests
	{
		#region Configuration
		private readonly SkillClient _skillClient;
		private readonly GameDataClient _gameDataClient;
		private readonly UserClient _userClient;
		private readonly GameClient _gameClient;

		public SkillClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_skillClient = testSugarClient.Skill;
			_gameDataClient = testSugarClient.GameData;
			_userClient = testSugarClient.User;
			_gameClient = testSugarClient.Game;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "SkillClientTests",
				Password = "SkillClientTestsPassword",
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
		public void CanCreateSkill()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanCreateSkill",
				ActorType = ActorType.User,
				Token = "CanCreateSkill",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanCreateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			Assert.Equal(skillRequest.Token, response.Token);
			Assert.Equal(skillRequest.ActorType, response.ActorType);
		}

		public void CanCreateGlobalSkill()
		{
			var skillRequest = new AchievementRequest()
			{
				Name = "CanCreateGlobalSkill",
				ActorType = ActorType.User,
				Token = "CanCreateGlobalSkill",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanCreateGlobalSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			Assert.Equal(skillRequest.Token, response.Token);
			Assert.Equal(skillRequest.ActorType, response.ActorType);
		}

		[Fact]
		public void CannotCreateDuplicateSkill()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateDuplicateSkill",
				ActorType = ActorType.User,
				Token = "CannotCreateDuplicateSkill",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateDuplicateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			_skillClient.Create(skillRequest);

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoName()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoName",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateSkillWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoToken()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateSkillWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoCompletionCriteria()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateSkillWithNoCompletionCriteria",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoCompletionCriteria",
				GameId = game.Id,
			};

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoAchievementCriteriaKey()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateSkillWithNoAchievementCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoAchievementCriteriaKey",
				GameId = game.Id,
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

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoAchievementCriteriaValue()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateSkillWithNoAchievementCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoAchievementCriteriaValue",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateSkillWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CannotCreateSkillWithNoAchievementCriteriaDataTypeMismatch()
		{
			var game = GetOrCreateGame("Create");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotCreateSkillWithNoAchievementCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoAchievementCriteriaDataTypeMismatch",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotCreateSkillWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Create(skillRequest));
		}

		[Fact]
		public void CanGetSkillsByGame()
		{
			var game = GetOrCreateGame("GameGet");

			var skillRequestOne = new AchievementRequest()
			{
				Name = "CanGetSkillsByGameOne",
				ActorType = ActorType.User,
				Token = "CanGetSkillsByGameOne",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetSkillsByGameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = _skillClient.Create(skillRequestOne);

			var skillRequestTwo = new AchievementRequest()
			{
				Name = "CanGetSkillsByGameTwo",
				ActorType = ActorType.User,
				Token = "CanGetSkillsByGameTwo",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetSkillsByGameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = _skillClient.Create(skillRequestTwo);

			var getSkill = _skillClient.GetByGame(game.Id);

			Assert.Equal(2, getSkill.Count());
		}

		[Fact]
		public void CanGetSkillByKeys()
		{
			var game = GetOrCreateGame("Get");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanGetSkillByKeys",
				ActorType = ActorType.User,
				Token = "CanGetSkillByKeys",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetSkillByKeys",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var getSkill = _skillClient.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.Equal(response.Name, getSkill.Name);
			Assert.Equal(skillRequest.Name, getSkill.Name);
		}

		[Fact]
		public void CannotGetNotExistingSkillByKeys()
		{
			var game = GetOrCreateGame("Get");

			var getSkill = _skillClient.GetById("CannotGetNotExistingSkillByKeys", game.Id);

			Assert.Null(getSkill);
		}

		[Fact]
		public void CannotGetSkillByEmptyToken()
		{
			var game = GetOrCreateGame("Get");

			Assert.Throws<Exception>(() => _skillClient.GetById("", game.Id));
		}

		[Fact]
		public void CanGetSkillByKeysThatContainSlashes()
		{
			var game = GetOrCreateGame("Get");

			var getSkill = _skillClient.GetById("Can/Get/Skill/By/Keys/That/Contain/Slashes", game.Id);

			Assert.Null(getSkill);
		}

		[Fact]
		public void CanGetGlobalSkillByToken()
		{
			var skillRequest = new AchievementRequest()
			{
				Name = "CanGetGlobalSkillByToken",
				ActorType = ActorType.User,
				Token = "CanGetGlobalSkillByToken",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetGlobalSkillByToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var getSkill = _skillClient.GetGlobalById(skillRequest.Token);

			Assert.Equal(response.Name, getSkill.Name);
			Assert.Equal(skillRequest.Name, getSkill.Name);
		}

		[Fact]
		public void CannotGetNotExistingGlobalSkillByKeys()
		{
			var getSkill = _skillClient.GetGlobalById("CannotGetNotExistingGlobalSkillByKeys");

			Assert.Null(getSkill);
		}

		[Fact]
		public void CannotGetGlobalSkillByEmptyToken()
		{
			Assert.Throws<Exception>(() => _skillClient.GetGlobalById(""));
		}

		[Fact]
		public void CanGetGlobalSkillByKeysThatContainSlashes()
		{
			var getSkill = _skillClient.GetGlobalById("Can/Get/Skill/By/Keys/That/Contain/Slashes");

			Assert.Null(getSkill);
		}

		[Fact]
		public void CannotGetBySkillsByNotExistingGameId()
		{
			var getSkills = _skillClient.GetByGame(-1);

			Assert.Empty(getSkills);
		}

		[Fact]
		public void CanUpdateSkill()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanUpdateSkill",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateSkill",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanUpdateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CanUpdateSkill Updated",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateSkill",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanUpdateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			_skillClient.Update(updateRequest);

			var updateResponse = _skillClient.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.NotEqual(response.Name, updateResponse.Name);
			Assert.Equal("CanUpdateSkill Updated", updateResponse.Name);
		}

		[Fact]
		public void CannotUpdateSkillToDuplicateName()
		{
			var game = GetOrCreateGame("Update");

			var skillRequestOne = new AchievementRequest()
			{
				Name = "CannotUpdateSkillToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameOne",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillToDuplicateNameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = _skillClient.Create(skillRequestOne);

			var skillRequestTwo = new AchievementRequest()
			{
				Name = "CannotUpdateSkillToDuplicateNameTwo",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameTwo",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = _skillClient.Create(skillRequestTwo);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameTwo",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateNonExistingSkill()
		{
			var game = GetOrCreateGame("Update");

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateNonExistingSkill",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateNonExistingSkill",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateNonExistingSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateAchievemenWithNoName()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
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

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
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

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateSkillWithNoToken()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoToken",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoToken",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateSkillWithNoCompletionCriteria()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoCompletionCriteria",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoCompletionCriteria",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoCompletionCriteria",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoCompletionCriteria",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoCompletionCriteria",
				GameId = game.Id,
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateSkillWithNoAchievementCriteriaKey()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaKey",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaKey",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoAchievementCriteriaKey",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaKey",
				GameId = game.Id,
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

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateSkillWithNoAchievementCriteriaValue()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaValue",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaValue",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaValue",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoAchievementCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch()
		{
			var game = GetOrCreateGame("Update");

			var skillRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var updateRequest = new AchievementRequest()
			{
				Name = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CannotUpdateSkillWithNoAchievementCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<Exception>(() => _skillClient.Update(updateRequest));
		}

		[Fact]
		public void CanDeleteSkill()
		{
			var game = GetOrCreateGame("Delete");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanDeleteSkill",
				ActorType = ActorType.User,
				Token = "CanDeleteSkill",
				GameId = game.Id,
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanDeleteSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var getSkill = _skillClient.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.NotNull(getSkill);

			_skillClient.Delete(skillRequest.Token, skillRequest.GameId.Value);

			getSkill = _skillClient.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.Null(getSkill);
		}

		[Fact]
		public void CannotDeleteNonExistingSkill()
		{
			var game = GetOrCreateGame("Delete");

			_skillClient.Delete("CannotDeleteNonExistingSkill", game.Id);
		}

		[Fact]
		public void CannotDeleteSkillByEmptyToken()
		{
			var game = GetOrCreateGame("Delete");

			Assert.Throws<Exception>(() => _skillClient.Delete("", game.Id));
		}

		[Fact]
		public void CanDeleteGlobalSkill()
		{
			var skillRequest = new AchievementRequest()
			{
				Name = "CanDeleteGlobalSkill",
				ActorType = ActorType.User,
				Token = "CanDeleteGlobalSkill",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanDeleteGlobalSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var getSkill = _skillClient.GetGlobalById(skillRequest.Token);

			Assert.NotNull(getSkill);

			_skillClient.DeleteGlobal(skillRequest.Token);

			getSkill = _skillClient.GetGlobalById(skillRequest.Token);

			Assert.Null(getSkill);
		}

		[Fact]
		public void CannotDeleteNonExistingGlobalSkill()
		{
			_skillClient.DeleteGlobal("CannotDeleteNonExistingGlobalSkill");
		}

		[Fact]
		public void CannotDeleteGlobalSkillByEmptyToken()
		{
			Assert.Throws<Exception>(() => _skillClient.DeleteGlobal(""));
		}

		[Fact]
		public void CanGetGlobalSkillProgress()
		{
			var user = GetOrCreateUser("ProgressGet");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanGetGlobalSkillProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalSkillProgress",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetGlobalSkillProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var progressGame = _skillClient.GetGlobalProgress(user.Id);
			Assert.Equal(1, progressGame.Count());

			var progressSkill = _skillClient.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.Equal(0, progressSkill.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetGlobalSkillProgress",
				Value = "1",
				ActorId = user.Id,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressSkill = _skillClient.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.Equal(1, progressSkill.Progress);
		}

		[Fact]
		public void CannotGetNotExistingGlobalSkillProgress()
		{
			var user = GetOrCreateUser("ProgressGet");

			Assert.Throws<Exception>(() => _skillClient.GetGlobalSkillProgress("CannotGetNotExistingGlobalSkillProgress", user.Id));
		}

		[Fact]
		public void CanGetSkillProgress()
		{
			var user = GetOrCreateUser("ProgressGet");
			var game = GetOrCreateGame("ProgressGet");

			var skillRequest = new AchievementRequest()
			{
				Name = "CanGetSkillProgress",
				GameId = game.Id,
				ActorType = ActorType.Undefined,
				Token = "CanGetSkillProgress",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						Key = "CanGetSkillProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = _skillClient.Create(skillRequest);

			var progressGame = _skillClient.GetGameProgress(game.Id, user.Id);
			Assert.Equal(1, progressGame.Count());

			var progressSkill = _skillClient.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.Equal(0, progressSkill.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetSkillProgress",
				Value = "1",
				ActorId = user.Id,
				GameId = game.Id,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressSkill = _skillClient.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.Equal(1, progressSkill.Progress);
		}

		[Fact]
		public void CannotGetNotExistingSkillProgress()
		{
			var user = GetOrCreateUser("ProgressGet");
			var game = GetOrCreateGame("ProgressGet");

			Assert.Throws<Exception>(() => _skillClient.GetSkillProgress("CannotGetNotExistingSkillProgress", game.Id, user.Id));
		}
		#endregion
		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "SkillControllerTests" + suffix ?? $"_{suffix}";
			var users = _userClient.Get(name, true);
			ActorResponse user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = _userClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return user;
		}

		private GameResponse GetOrCreateGame(string suffix)
		{
			string name = "SkillControllerTests" + suffix ?? $"_{suffix}";
			var games = _gameClient.Get(name);
			GameResponse game;

			if (games.Any())
			{
				game = games.Single();
			}
			else
			{
				game = _gameClient.Create(new GameRequest
				{
					Name = name
				});
			}

			return game;
		}
		#endregion
	}
}
