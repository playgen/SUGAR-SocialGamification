using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;
using System;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class AchievementClientTests 
	{
		#region Configuration
		private readonly AchievementClient _achievementClient;
		private readonly GameDataClient _gameDataClient;
		private readonly UserClient _userClient;
		private readonly GameClient _gameClient;

		public AchievementClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_achievementClient = testSugarClient.Achievement;
			_gameDataClient = testSugarClient.GameData;
			_userClient = testSugarClient.User;
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
		[Test]
		public void CanCreateAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanCreateAchievement",
				ActorType = ActorType.User,
				Token = "CanCreateAchievement",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest
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

			Assert.AreEqual(achievementRequest.Token, response.Token);
			Assert.AreEqual(achievementRequest.ActorType, response.ActorType);
		}

		public void CanCreateGlobalAchievement()
		{
			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanCreateGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanCreateGlobalAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.AreEqual(achievementRequest.Token, response.Token);
			Assert.AreEqual(achievementRequest.ActorType, response.ActorType);
		}

		[Test]
		public void CannotCreateDuplicateAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateDuplicateAchievement",
				ActorType = ActorType.User,
				Token = "CannotCreateDuplicateAchievement",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoName()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteria",
				GameId = game.Id,
			};

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Create");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Create(achievementRequest));
		}

		[Test]
		public void CanGetAchievementsByGame()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "GameGet");

			var achievementRequestOne = new EvaluationCreateRequest()
			{
				Name = "CanGetAchievementsByGameOne",
				ActorType = ActorType.User,
				Token = "CanGetAchievementsByGameOne",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetAchievementsByGameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = _achievementClient.Create(achievementRequestOne);

			var achievementRequestTwo = new EvaluationCreateRequest()
			{
				Name = "CanGetAchievementsByGameTwo",
				ActorType = ActorType.User,
				Token = "CanGetAchievementsByGameTwo",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetAchievementsByGameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = _achievementClient.Create(achievementRequestTwo);

			var getAchievement = _achievementClient.GetByGame(game.Id);

			Assert.AreEqual(2, getAchievement.Count());
		}

		[Test]
		public void CanGetAchievementByKeys()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetAchievementByKeys",
				ActorType = ActorType.User,
				Token = "CanGetAchievementByKeys",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.AreEqual(response.Name, getAchievement.Name);
			Assert.AreEqual(achievementRequest.Name, getAchievement.Name);
		}

		[Test]
		public void CannotGetNotExistingAchievementByKeys()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var getAchievement = _achievementClient.GetById("CannotGetNotExistingAchievementByKeys", game.Id);

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotGetAchievementByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			Assert.Throws<ClientException>(() => _achievementClient.GetById("", game.Id));
		}

		[Test]
		public void CanGetAchievementByKeysThatContainSlashes()
		{
            // todo this test seems incorrect
			var game = Helpers.GetOrCreateGame(_gameClient, "Get");

			var getAchievement = _achievementClient.GetById("Can/Get/Achievement/By/Keys/That/Contain/Slashes", game.Id);

			Assert.Null(getAchievement);
		}

		[Test]
		public void CanGetGlobalAchievementByToken()
		{
			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetGlobalAchievementByToken",
				ActorType = ActorType.User,
				Token = "CanGetGlobalAchievementByToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			Assert.AreEqual(response.Name, getAchievement.Name);
			Assert.AreEqual(achievementRequest.Name, getAchievement.Name);
		}

		[Test]
		public void CannotGetNotExistingGlobalAchievementByKeys()
		{
			var getAchievement = _achievementClient.GetGlobalById("CannotGetNotExistingGlobalAchievementByKeys");

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotGetGlobalAchievementByEmptyToken()
		{
			Assert.Throws<ClientException>(() => _achievementClient.GetGlobalById(""));
		}

		[Test]
		public void CanGetGlobalAchievementByKeysThatContainSlashes()
		{
            // todo this test seems incorrect
			var getAchievement = _achievementClient.GetGlobalById("Can/Get/Achievement/By/Keys/That/Contain/Slashes");

            // todo should this not be an exception?
			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotGetByAchievementsByNotExistingGameId()
		{
			var getAchievements = _achievementClient.GetByGame(-1);

			Assert.IsEmpty(getAchievements);
		}

		[Test]
		public void CanUpdateAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanUpdateAchievement",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var updateRequest = new EvaluationUpdateRequest
			{
                Id = response.Id,
				Name = "CanUpdateAchievement Updated",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
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

			Assert.AreNotEqual(response.Name, updateResponse.Name);
			Assert.AreEqual("CanUpdateAchievement Updated", updateResponse.Name);
		}

		[Test]
		public void CannotUpdateAchievementToDuplicateName()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequestOne = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameOne",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var achievementRequestTwo = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievementToDuplicateNameTwo",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameTwo",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = responseTwo.Id,
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameTwo",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = responseTwo.EvaluationCriterias[0].Id,
                        Key = "CannotUpdateAchievementToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateNonExistingAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = int.MaxValue,
				Name = "CannotUpdateNonExistingAchievement",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateNonExistingAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = int.MaxValue,
						Key = "CannotUpdateNonExistingAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievemenWithNoName()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
                        Key = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievementWithNoToken",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
                        Key = "CannotUpdateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteria",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteria",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateAchievementWithNoEvaluationCriteria",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteria",
				GameId = game.Id,
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
                        ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
                        Key = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Update");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = _achievementClient.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
                        Key = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = GameDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<ClientException>(() => _achievementClient.Update(updateRequest));
		}

		[Test]
		public void CanDeleteAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Delete");

			var achievementRequest = new EvaluationCreateRequest()
			{
                Token = "CanDeleteAchievement",
                GameId = game.Id,
                Name = "CanDeleteAchievement",
				ActorType = ActorType.User,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

            var response = CreateAchievement(achievementRequest);

			var getAchievement = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.NotNull(getAchievement);

			_achievementClient.Delete(achievementRequest.Token, achievementRequest.GameId.Value);

			getAchievement = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.Null(getAchievement);
		}

        [Test]
		public void CannotDeleteNonExistingAchievement()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Delete");

			_achievementClient.Delete("CannotDeleteNonExistingAchievement", game.Id);
		}

		[Test]
		public void CannotDeleteAchievementByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(_gameClient, "Delete");

			Assert.Throws<ClientException>(() => _achievementClient.Delete("", game.Id));
		}

		[Test]
		public void CanDeleteGlobalAchievement()
		{
			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanDeleteGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanDeleteGlobalAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

		[Test]
		public void CannotDeleteNonExistingGlobalAchievement()
		{
			_achievementClient.DeleteGlobal("CannotDeleteNonExistingGlobalAchievement");
		}

		[Test]
		public void CannotDeleteGlobalAchievementByEmptyToken()
		{
			Assert.Throws<ClientException>(() => _achievementClient.DeleteGlobal(""));
		}

		[Test]
		public void CanGetGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "ProgressGet");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetGlobalAchievementProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalAchievementProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var progressGame = _achievementClient.GetGlobalProgress(user.Id);
			Assert.AreEqual(1, progressGame.Count());

			var progressAchievement = _achievementClient.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.AreEqual(0, progressAchievement.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetGlobalAchievementProgress",
				Value = "1",
				ActorId = user.Id,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressAchievement = _achievementClient.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.AreEqual(1, progressAchievement.Progress);
		}

		[Test]
		public void CannotGetNotExistingGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "ProgressGet");

			Assert.Throws<ClientException>(() => _achievementClient.GetGlobalAchievementProgress("CannotGetNotExistingGlobalAchievementProgress", user.Id));
		}

		[Test]
		public void CanGetAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "ProgressGet");
			var game = Helpers.GetOrCreateGame(_gameClient, "ProgressGet");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetAchievementProgress",
				GameId = game.Id,
				ActorType = ActorType.Undefined,
				Token = "CanGetAchievementProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
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

			var progressGame = _achievementClient.GetGameProgress(game.Id, user.Id);
			Assert.AreEqual(1, progressGame.Count());

			var progressAchievement = _achievementClient.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(0, progressAchievement.Progress);

			var gameData = new GameDataRequest()
			{
				Key = "CanGetAchievementProgress",
				Value = "1",
				ActorId = user.Id,
				GameId = game.Id,
				GameDataType = GameDataType.Float
			};

			_gameDataClient.Add(gameData);

			progressAchievement = _achievementClient.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(1, progressAchievement.Progress);
		}

		[Test]
		public void CannotGetNotExistingAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(_userClient, "ProgressGet");
			var game = Helpers.GetOrCreateGame(_gameClient, "ProgressGet");

			Assert.Throws<ClientException>(() => _achievementClient.GetAchievementProgress("CannotGetNotExistingAchievementProgress", game.Id, user.Id));
		}
        #endregion

        #region Helpers
        private object CreateAchievement(EvaluationCreateRequest achievementRequest)
        {
            var getAchievement = _achievementClient.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

            if (getAchievement != null)
            {
                _achievementClient.Delete(achievementRequest.Token, achievementRequest.GameId.Value);
            }
            
            var response = _achievementClient.Create(achievementRequest);
            
            return response;
        }
        #endregion
    }
}
