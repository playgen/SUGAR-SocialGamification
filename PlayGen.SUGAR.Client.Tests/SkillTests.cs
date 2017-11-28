using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class SkillClientTests : Evaluations
	{
		[Fact]
		public void CanDisableNotifications()
		{
			// Assign
			var loggedInAccount = LoginAdmin();
			var key = "CanDisableNotifications";

			SUGARClient.Skill.EnableNotifications(true);

			EvaluationNotification notification;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			SUGARClient.Skill.EnableNotifications(false);

			var skill = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(skill, loggedInAccount.User.Id);

			// Act
			var didGetnotification = SUGARClient.Skill.TryGetPendingNotification(out notification);

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

			SUGARClient.Skill.EnableNotifications(true);
			var skill = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(skill, loggedInAccount.User.Id);

			// Act
			EvaluationNotification notification;
			var didGetnotification = false;
			EvaluationNotification gotNotification = null;
			var didGetSpecificConfiguration = false;

			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
				didGetnotification = true;
				gotNotification = notification;
				didGetSpecificConfiguration |= notification.Name == skill.Name;
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

			SUGARClient.Skill.EnableNotifications(true);
			var skill = CreateGenericEvaluation(key);

			CompleteGenericEvaluation(skill, loggedInAccount.User.Id);

			EvaluationNotification notification;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			CompleteGenericEvaluation(skill, loggedInAccount.User.Id);

			// Act
			var didGetSpecificConfiguration = false;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
				didGetSpecificConfiguration |= notification.Name == skill.Name;
			}

			// Assert
			Assert.False(didGetSpecificConfiguration);
		}

		[Fact]
		public void CanGetGlobalSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(SkillClientTests)}_ProgressGet");

			var skillRequest = new EvaluationCreateRequest()
										{
											Name = "CanGetGlobalSkillProgress",
											ActorType = ActorType.Undefined,
											Token = "CanGetGlobalSkillProgress",
											EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
																	{
																		new EvaluationCriteriaCreateRequest()
																			{
																				EvaluationDataKey  ="CanGetGlobalSkillProgress",
																				ComparisonType = ComparisonType.Equals,
																				CriteriaQueryType = CriteriaQueryType.Any,
																				EvaluationDataType = EvaluationDataType.Float,
																				Scope = CriteriaScope.Actor,
																				Value = "1"
																			}
																	},
										};

			var response = SUGARClient.Skill.Create(skillRequest);

			var progressGame = SUGARClient.Skill.GetGlobalProgress(user.Id);
			Assert.NotEmpty(progressGame);

			var progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.Equal(0, progressSkill.Progress);

			var gameData = new EvaluationDataRequest()
								{
									Key = "CanGetGlobalSkillProgress",
									Value = "1",
									CreatingActorId = user.Id,
									EvaluationDataType = EvaluationDataType.Float
								};

			SUGARClient.GameData.Add(gameData);

			progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.True(progressSkill.Progress >= 1);
		}

		[Fact]
		public void CannotGetNotExistingGlobalSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(SkillClientTests)}_ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetGlobalSkillProgress("CannotGetNotExistingGlobalSkillProgress", user.Id));
		}

		[Fact]
		public void CanGetSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(SkillClientTests)}_ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(SkillClientTests)}_ProgressGet");

			var skillRequest = new EvaluationCreateRequest()
										{
											Name = "CanGetSkillProgress",
											GameId = game.Id,
											ActorType = ActorType.Undefined,
											Token = "CanGetSkillProgress",
											EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
																	{
																		new EvaluationCriteriaCreateRequest()
																			{
																				EvaluationDataKey  ="CanGetSkillProgress",
																				ComparisonType = ComparisonType.Equals,
																				CriteriaQueryType = CriteriaQueryType.Any,
																				EvaluationDataType = EvaluationDataType.Float,
																				Scope = CriteriaScope.Actor,
																				Value = "1"
																			}
																	},
										};

			var response = SUGARClient.Skill.Create(skillRequest);

			var progressGame = SUGARClient.Skill.GetGameProgress(game.Id, user.Id);
			Assert.Equal(1, progressGame.Count());

			var progressSkill = SUGARClient.Skill.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.Equal(0, progressSkill.Progress);

			var gameData = new EvaluationDataRequest()
								{
									Key = "CanGetSkillProgress",
									Value = "1",
									CreatingActorId = user.Id,
									GameId = game.Id,
									EvaluationDataType = EvaluationDataType.Float
								};

			SUGARClient.GameData.Add(gameData);

			progressSkill = SUGARClient.Skill.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.Equal(1, progressSkill.Progress);
		}

		[Fact]
		public void CannotGetNotExistingSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(SkillClientTests)}_ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(SkillClientTests)}_ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetSkillProgress("CannotGetNotExistingSkillProgress", game.Id, user.Id));
		}

		#region Helpers
		protected override EvaluationResponse CreateEvaluation(EvaluationCreateRequest skillRequest)
		{
			var getSkill = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId ?? 0);

			if (getSkill != null)
			{
				if (skillRequest.GameId.HasValue)
				{
					SUGARClient.Skill.Delete(skillRequest.Token, skillRequest.GameId.Value);
				}
				else
				{
					SUGARClient.Skill.DeleteGlobal(skillRequest.Token);
				}
			}

			var response = SUGARClient.Skill.Create(skillRequest);

			return response;
		}
		#endregion
	}
}
