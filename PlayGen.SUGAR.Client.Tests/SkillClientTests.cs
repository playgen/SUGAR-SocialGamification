using System.Linq;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class SkillClientTests : EvaluationClientTests
	{
		[Fact]
		public void CanDisableNotifications()
		{
			// Assign
			var key = "Skill_CanDisableNotifications";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			SUGARClient.Skill.EnableNotifications(true);

			EvaluationNotification notification;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			SUGARClient.Skill.EnableNotifications(false);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

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
			var key = "Skill_CanGetNotifications";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			SUGARClient.Skill.EnableNotifications(true);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			// Act
			EvaluationNotification notification;
			var didGetnotification = false;
			EvaluationNotification gotNotification = null;
			var didGetSpecificConfiguration = false;

			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
				didGetnotification = true;
				gotNotification = notification;
				didGetSpecificConfiguration |= notification.Name == key;
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
			var key = "Skill_DontGetAlreadyRecievedNotifications";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			SUGARClient.Skill.EnableNotifications(true);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			EvaluationNotification notification;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			// Act
			var didGetSpecificConfiguration = false;
			while (SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
				didGetSpecificConfiguration |= notification.Name == key;
			}

			// Assert
			Assert.False(didGetSpecificConfiguration);
		}

		[Fact]
		public void CanGetGlobalSkillProgress()
		{
			var key = "Skill_CanGetGlobalSkillProgress";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var progressGame = SUGARClient.Skill.GetGlobalProgress(loggedInAccount.User.Id);
			Assert.NotEmpty(progressGame);

			var progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id);
			Assert.Equal(0, progressSkill.Progress);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id);

			progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id);
			Assert.True(progressSkill.Progress >= 1);
		}

		[Fact]
		public void CannotGetNotExistingGlobalSkillProgress()
		{
			var key = "Skill_CannotGetNotExistingGlobalSkillProgress";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id));
		}

		[Fact]
		public void CanGetSkillProgress()
		{
			var key = "Skill_CanGetSkillProgress";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var progressGame = SUGARClient.Skill.GetGameProgress(game.Id, loggedInAccount.User.Id);
			Assert.Equal(1, progressGame.Count());

			var progressSkill = SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id);
			Assert.Equal(0, progressSkill.Progress);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			progressSkill = SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id);
			Assert.Equal(1, progressSkill.Progress);
		}

		[Fact]
		public void CannotGetNotExistingSkillProgress()
		{
			var key = "Skill_CannotGetNotExistingSkillProgress";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id));
		}
	}
}
