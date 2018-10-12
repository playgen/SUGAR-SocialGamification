using System.Linq;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Authorization;

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
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			Fixture.SUGARClient.Skill.EnableNotifications(true);

			EvaluationNotification notification;
			while (Fixture.SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			Fixture.SUGARClient.Skill.EnableNotifications(false);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			// Act
			var didGetnotification = Fixture.SUGARClient.Skill.TryGetPendingNotification(out notification);

			// Assert
			Assert.False(didGetnotification);
			Assert.Null(notification);
		}

		[Fact]
		public void CanGetNotifications()
		{
			// Assign
			var key = "Skill_CanGetNotifications";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			Fixture.SUGARClient.Skill.EnableNotifications(true);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			// Act
			var didGetnotification = false;
			EvaluationNotification gotNotification = null;
			var didGetSpecificConfiguration = false;

			while (Fixture.SUGARClient.Skill.TryGetPendingNotification(out var notification))
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
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			Fixture.SUGARClient.Skill.EnableNotifications(true);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			EvaluationNotification notification;
			while (Fixture.SUGARClient.Skill.TryGetPendingNotification(out notification))
			{
			}

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			// Act
			var didGetSpecificConfiguration = false;
			while (Fixture.SUGARClient.Skill.TryGetPendingNotification(out notification))
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
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var progressGame = Fixture.SUGARClient.Skill.GetGlobalProgress(loggedInAccount.User.Id);
			Assert.NotEmpty(progressGame);

			var progressSkill = Fixture.SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id);
			Assert.Equal(0, progressSkill.Progress);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, Platform.GlobalGameId);

			progressSkill = Fixture.SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id);
			Assert.True(progressSkill.Progress >= 1);
		}

		[Fact]
		public void CannotGetNotExistingGlobalSkillProgress()
		{
			var key = "Skill_CannotGetNotExistingGlobalSkillProgress";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Skill.GetGlobalSkillProgress(key, loggedInAccount.User.Id));
		}

		[Fact]
		public void CanGetSkillProgress()
		{
			var key = "Skill_CanGetSkillProgress";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var progressGame = Fixture.SUGARClient.Skill.GetGameProgress(game.Id, loggedInAccount.User.Id);
			Assert.Equal(1, progressGame.Count());

			var progressSkill = Fixture.SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id);
			Assert.Equal(0, progressSkill.Progress);

			CompleteGenericEvaluation(key, loggedInAccount.User.Id, game.Id);

			progressSkill = Fixture.SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id);
			Assert.Equal(1, progressSkill.Progress);
		}

		[Fact]
		public void CannotGetNotExistingSkillProgress()
		{
			var key = "Skill_CannotGetNotExistingSkillProgress";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Skill.GetSkillProgress(key, game.Id, loggedInAccount.User.Id));
		}

		public SkillClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
