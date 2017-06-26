using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class AchievementClientTests : Evaluations
	{
		[Test]
		public void CanDisableNotifications()
		{
			// Assign
			var key = "CanDisableNotifications";

			SUGARClient.Achievement.EnableNotifications(true);

			EvaluationNotification notification;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
			}

			SUGARClient.Achievement.EnableNotifications(false);

			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, LoggedInAccount.User.Id);

			// Act
			var didGetnotification = SUGARClient.Achievement.TryGetPendingNotification(out notification);

			// Assert
			Assert.IsFalse(didGetnotification);
			Assert.IsNull(notification);
		}

		[Test]
		public void CanGetNotifications()
		{
			// Assign
			var key = "CanGetNotifications";

			SUGARClient.Achievement.EnableNotifications(true);
			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, LoggedInAccount.User.Id);

			// Act
			EvaluationNotification notification;
			var didGetnotification = false;
			EvaluationNotification gotNotification = null;
			var didGetSpecificConfiguration = false;

			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
				didGetnotification = true;
				gotNotification = notification;
				didGetSpecificConfiguration |= notification.Name == achievement.Name;
			}

			// Assert
			Assert.IsTrue(didGetnotification);
			Assert.IsNotNull(gotNotification);

			Assert.IsTrue(didGetSpecificConfiguration);
		}

		[Test]
		public void DontGetAlreadyRecievedNotifications()
		{
			// Assign
			var key = "DontGetAlreadyRecievedNotifications";

			SUGARClient.Achievement.EnableNotifications(true);
			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, LoggedInAccount.User.Id);

			EvaluationNotification notification;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
			}

			CompleteGenericEvaluation(achievement, LoggedInAccount.User.Id);

			// Act
			var didGetSpecificConfiguration = false;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
				didGetSpecificConfiguration |= notification.Name == achievement.Name;

			// Assert
			Assert.IsFalse(didGetSpecificConfiguration);
		}

		[Test]
		public void CanCreateAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanCreateAchievement",
				ActorType = ActorType.User,
				Token = "CanCreateAchievement",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanCreateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			Assert.AreEqual(achievementRequest.Token, response.Token);
			Assert.AreEqual(achievementRequest.ActorType, response.ActorType);
		}

		public void CanCreateGlobalAchievement()
		{
			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanCreateGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanCreateGlobalAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanCreateGlobalAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			Assert.AreEqual(achievementRequest.Token, response.Token);
			Assert.AreEqual(achievementRequest.ActorType, response.ActorType);
		}

		[Test]
		public void CannotCreateDuplicateAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateDuplicateAchievement",
				ActorType = ActorType.User,
				Token = "CannotCreateDuplicateAchievement",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotCreateDuplicateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			SUGARClient.Achievement.Create(achievementRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotCreateAchievementWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotCreateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteria",
				GameId = game.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotCreateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotCreateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Create(achievementRequest));
		}

		[Test]
		public void CanGetAchievementsByGame()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "GameGet");

			var achievementRequestOne = new EvaluationCreateRequest
			{
				Name = "CanGetAchievementsByGameOne",
				ActorType = ActorType.User,
				Token = "CanGetAchievementsByGameOne",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetAchievementsByGameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var responseOne = SUGARClient.Achievement.Create(achievementRequestOne);

			var achievementRequestTwo = new EvaluationCreateRequest
			{
				Name = "CanGetAchievementsByGameTwo",
				ActorType = ActorType.User,
				Token = "CanGetAchievementsByGameTwo",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetAchievementsByGameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var responseTwo = SUGARClient.Achievement.Create(achievementRequestTwo);

			var getAchievement = SUGARClient.Achievement.GetByGame(game.Id);

			Assert.AreEqual(2, getAchievement.Count());
		}

		[Test]
		public void CanGetAchievementByKeys()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanGetAchievementByKeys",
				ActorType = ActorType.User,
				Token = "CanGetAchievementByKeys",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetAchievementByKeys",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var getAchievement = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.AreEqual(response.Name, getAchievement.Name);
			Assert.AreEqual(achievementRequest.Name, getAchievement.Name);
		}

		[Test]
		public void CannotGetNotExistingAchievementByKeys()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var getAchievement = SUGARClient.Achievement.GetById("CannotGetNotExistingAchievementByKeys", game.Id);

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotGetAchievementByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			Assert.Throws<ClientException>(() => SUGARClient.Achievement.GetById("", game.Id));
		}

		[Test]
		public void CanGetAchievementByKeysThatContainSlashes()
		{
			// todo this test seems incorrect
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.GetById("Can/Get/Achievement/By/Keys/That/Contain/Slashes", game.Id));
		}

		[Test]
		public void CanGetGlobalAchievementByToken()
		{
			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanGetGlobalAchievementByToken",
				ActorType = ActorType.User,
				Token = "CanGetGlobalAchievementByToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetGlobalAchievementByToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var getAchievement = SUGARClient.Achievement.GetGlobalById(achievementRequest.Token);

			Assert.AreEqual(response.Name, getAchievement.Name);
			Assert.AreEqual(achievementRequest.Name, getAchievement.Name);
		}

		[Test]
		public void CannotGetNotExistingGlobalAchievementByKeys()
		{
			var getAchievement = SUGARClient.Achievement.GetGlobalById("CannotGetNotExistingGlobalAchievementByKeys");

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotGetGlobalAchievementByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Achievement.GetGlobalById(""));
		}

		[Test]
		public void CanGetGlobalAchievementByKeysThatContainSlashes()
		{
			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.GetGlobalById("Can/Get/Achievement/By/Keys/That/Contain/Slashes"));
		}

		[Test]
		public void CannotGetByAchievementsByNotExistingGameId()
		{
			var getAchievements = SUGARClient.Achievement.GetByGame(-1);

			Assert.IsEmpty(getAchievements);
		}

		[Test]
		public void CanUpdateAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanUpdateAchievement",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanUpdateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CanUpdateAchievement Updated",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CanUpdateAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			SUGARClient.Achievement.Update(updateRequest);

			var updateResponse = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.AreNotEqual(response.Name, updateResponse.Name);
			Assert.AreEqual("CanUpdateAchievement Updated", updateResponse.Name);
		}

		[Test]
		public void CannotUpdateAchievementToDuplicateToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequestOne = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameOne",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementToDuplicateNameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var responseOne = SUGARClient.Achievement.Create(achievementRequestOne);

			var achievementRequestTwo = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementToDuplicateNameTwo",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameTwo",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var responseTwo = SUGARClient.Achievement.Create(achievementRequestTwo);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = responseTwo.Id,
				Name = "CannotUpdateAchievementToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementToDuplicateNameOne",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = responseTwo.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CannotUpdateAchievementToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateNonExistingAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = int.MaxValue,
				Name = "CannotUpdateNonExistingAchievement",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateNonExistingAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = int.MaxValue,
						EvaluationDataKey = "CannotUpdateNonExistingAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievemenWithNoName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoToken",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CannotUpdateAchievementWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CannotUpdateAchievementWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteria",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteria",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteria",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteria",
				GameId = game.Id
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var updateRequest = new EvaluationUpdateRequest
			{
				Id = response.Id,
				Name = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>
				{
					new EvaluationCriteriaUpdateRequest
					{
						Id = response.EvaluationCriterias[0]
							.Id,
						EvaluationDataKey = "CannotUpdateAchievementWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				}
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.Update(updateRequest));
		}

		[Test]
		public void CanDeleteAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			var achievementRequest = new EvaluationCreateRequest
			{
				Token = "CanDeleteAchievement",
				GameId = game.Id,
				Name = "CanDeleteAchievement",
				ActorType = ActorType.User,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanDeleteAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = CreateEvaluation(achievementRequest);

			var getAchievement = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.NotNull(getAchievement);

			SUGARClient.Achievement.Delete(achievementRequest.Token, achievementRequest.GameId.Value);

			getAchievement = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId.Value);

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotDeleteNonExistingAchievement()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.Delete("CannotDeleteNonExistingAchievement", game.Id));
		}

		[Test]
		public void CannotDeleteAchievementByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			Assert.Throws<ClientException>(() => SUGARClient.Achievement.Delete("", game.Id));
		}

		[Test]
		public void CanDeleteGlobalAchievement()
		{
			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanDeleteGlobalAchievement",
				ActorType = ActorType.User,
				Token = "CanDeleteGlobalAchievement",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanDeleteGlobalAchievement",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var getAchievement = SUGARClient.Achievement.GetGlobalById(achievementRequest.Token);

			Assert.NotNull(getAchievement);

			SUGARClient.Achievement.DeleteGlobal(achievementRequest.Token);

			getAchievement = SUGARClient.Achievement.GetGlobalById(achievementRequest.Token);

			Assert.Null(getAchievement);
		}

		[Test]
		public void CannotDeleteNonExistingGlobalAchievement()
		{
			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.DeleteGlobal("CannotDeleteNonExistingGlobalAchievement"));
		}

		[Test]
		public void CannotDeleteGlobalAchievementByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Achievement.DeleteGlobal(""));
		}

		[Test]
		public void CanGetGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanGetGlobalAchievementProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalAchievementProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetGlobalAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var progressGame = SUGARClient.Achievement.GetGlobalProgress(user.Id);
			Assert.GreaterOrEqual(progressGame.Count(), 1);

			var progressAchievement = SUGARClient.Achievement.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.AreEqual(0, progressAchievement.Progress);

			var gameData = new EvaluationDataRequest
			{
				Key = "CanGetGlobalAchievementProgress",
				Value = "1",
				CreatingActorId = user.Id,
				EvaluationDataType = EvaluationDataType.Float
			};

			SUGARClient.GameData.Add(gameData);

			progressAchievement = SUGARClient.Achievement.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.GreaterOrEqual(progressAchievement.Progress, 1);
		}

		[Test]
		public void CannotGetNotExistingGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");

			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.GetGlobalAchievementProgress("CannotGetNotExistingGlobalAchievementProgress",
					user.Id));
		}

		[Test]
		public void CanGetAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "ProgressGet");

			var achievementRequest = new EvaluationCreateRequest
			{
				Name = "CanGetAchievementProgress",
				GameId = game.Id,
				ActorType = ActorType.Undefined,
				Token = "CanGetAchievementProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>
				{
					new EvaluationCriteriaCreateRequest
					{
						EvaluationDataKey = "CanGetAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				}
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var progressGame = SUGARClient.Achievement.GetGameProgress(game.Id, user.Id);
			Assert.AreEqual(1, progressGame.Count());

			var progressAchievement = SUGARClient.Achievement.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(0, progressAchievement.Progress);

			var gameData = new EvaluationDataRequest
			{
				Key = "CanGetAchievementProgress",
				Value = "1",
				CreatingActorId = user.Id,
				GameId = game.Id,
				EvaluationDataType = EvaluationDataType.Float
			};

			SUGARClient.GameData.Add(gameData);

			progressAchievement = SUGARClient.Achievement.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(1, progressAchievement.Progress);
		}

		[Test]
		public void CannotGetNotExistingAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "ProgressGet");

			Assert.Throws<ClientHttpException>(
				() => SUGARClient.Achievement.GetAchievementProgress("CannotGetNotExistingAchievementProgress", game.Id, user.Id));
		}

		#region Helpers

		protected override EvaluationResponse CreateEvaluation(EvaluationCreateRequest achievementRequest)
		{
			var getAchievement = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId ?? 0);

			if (getAchievement != null)
				if (achievementRequest.GameId.HasValue)
					SUGARClient.Achievement.Delete(achievementRequest.Token, achievementRequest.GameId.Value);
				else
					SUGARClient.Achievement.DeleteGlobal(achievementRequest.Token);

			var response = SUGARClient.Achievement.Create(achievementRequest);

			return response;
		}

		#endregion
	}
}