using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class AchievementClientTests : Evaluations
	{ 
		[Fact]
		public void CanDisableNotifications()
		{
			// Assign
			var loggedInAccount = LoginAdmin();
			var key = "CanDisableNotifications";

			SUGARClient.Achievement.EnableNotifications(true);

			EvaluationNotification notification;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
			}

			SUGARClient.Achievement.EnableNotifications(false);

			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, loggedInAccount.User.Id);

			// Act
			var didGetnotification = SUGARClient.Achievement.TryGetPendingNotification(out notification);

			// Assert
			Assert.False(didGetnotification);
			Assert.Null(notification);
		}

		[Fact]
		public void CanGetNotifications()
		{
			// Assign
			var loggedInAccount = LoginAdmin();
			var key = "CanGetNotifications";

			SUGARClient.Achievement.EnableNotifications(true);
			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, loggedInAccount.User.Id);

			// Act
			EvaluationNotification notification;
			var didGetnotification = false;
			EvaluationNotification gotNotification= null;
			var didGetSpecificConfiguration = false;

			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
				didGetnotification = true;
				gotNotification = notification; 
				didGetSpecificConfiguration |= notification.Name == achievement.Name;
			}

			// Assert
			Assert.True(didGetnotification);
			Assert.NotNull(gotNotification);

			Assert.True(didGetSpecificConfiguration);
		}

		[Fact]
		public void DontGetAlreadyRecievedNotifications()
		{
			// Assign
			var loggedInAccount = LoginAdmin();
			var key = "DontGetAlreadyRecievedNotifications";

			SUGARClient.Achievement.EnableNotifications(true);
			var achievement = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(achievement, loggedInAccount.User.Id);

			EvaluationNotification notification;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
			}

			CompleteGenericEvaluation(achievement, loggedInAccount.User.Id);

			// Act
			var didGetSpecificConfiguration = false;
			while (SUGARClient.Achievement.TryGetPendingNotification(out notification))
			{
				didGetSpecificConfiguration |= notification.Name == achievement.Name;
			}

			// Assert
			Assert.False(didGetSpecificConfiguration);
		}

		[Fact]
		public void CanGetGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(AchievementClientTests)}_ProgressGet");

			var achievementRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetGlobalAchievementProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalAchievementProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataKey  ="CanGetGlobalAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var progressGame = SUGARClient.Achievement.GetGlobalProgress(user.Id);
			Assert.NotEmpty(progressGame);

			var progressAchievement = SUGARClient.Achievement.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.Equal(0, progressAchievement.Progress);

			var gameData = new EvaluationDataRequest()
			{
				Key  ="CanGetGlobalAchievementProgress",
				Value = "1",
				CreatingActorId = user.Id,
				EvaluationDataType = EvaluationDataType.Float
			};

			SUGARClient.GameData.Add(gameData);

			progressAchievement = SUGARClient.Achievement.GetGlobalAchievementProgress(response.Token, user.Id);
			Assert.True(progressAchievement.Progress >= 1);
		}

		[Fact]
		public void CannotGetNotExistingGlobalAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(AchievementClientTests)}_ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.GetGlobalAchievementProgress("CannotGetNotExistingGlobalAchievementProgress", user.Id));
		}

		[Fact]
		public void CanGetAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(AchievementClientTests)}_ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(AchievementClientTests)}_ProgressGet");

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
						EvaluationDataKey  ="CanGetAchievementProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						EvaluationDataType = EvaluationDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = SUGARClient.Achievement.Create(achievementRequest);

			var progressGame = SUGARClient.Achievement.GetGameProgress(game.Id, user.Id);
			Assert.Equal(1, progressGame.Count());

			var progressAchievement = SUGARClient.Achievement.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.Equal(0, progressAchievement.Progress);

			var gameData = new EvaluationDataRequest()
			{
				Key  ="CanGetAchievementProgress",
				Value = "1",
				CreatingActorId = user.Id,
				GameId = game.Id,
				EvaluationDataType = EvaluationDataType.Float
			};

			SUGARClient.GameData.Add(gameData);

			progressAchievement = SUGARClient.Achievement.GetAchievementProgress(response.Token, game.Id, user.Id);
			Assert.Equal(1, progressAchievement.Progress);
		}

		[Fact]
		public void CannotGetNotExistingAchievementProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(AchievementClientTests)}_ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(AchievementClientTests)}_ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Achievement.GetAchievementProgress("CannotGetNotExistingAchievementProgress", game.Id, user.Id));
		}

		#region Helpers
		protected override EvaluationResponse CreateEvaluation(EvaluationCreateRequest achievementRequest)
		{
			var getAchievement = SUGARClient.Achievement.GetById(achievementRequest.Token, achievementRequest.GameId ?? 0);

			if (getAchievement != null)
			{
				if (achievementRequest.GameId.HasValue)
				{
					SUGARClient.Achievement.Delete(achievementRequest.Token, achievementRequest.GameId.Value);
				}
				else
				{
					SUGARClient.Achievement.DeleteGlobal(achievementRequest.Token);
				}
			}
			
			var response = SUGARClient.Achievement.Create(achievementRequest);
			
			return response;
		}
		#endregion
	}
}
