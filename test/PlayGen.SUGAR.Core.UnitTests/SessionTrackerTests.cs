using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using PlayGen.SUGAR.Core.Sessions;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests
{
	[Collection("Project Fixture Collection")]
	public class SessionTrackerTests
	{
		private TimeSpan DefaultTimeoutCheckInterval => new TimeSpan(0, 10, 0);

		/// <summary>
		///     Is game id/user combination removed and not retrievable on session ended
		/// </summary>
		[Fact]
		public void CanEndSession()
		{
			// Arrange
			var sessionTracker = new SessionTracker(DefaultTimeoutCheckInterval, DefaultTimeoutCheckInterval);
			var game = Helpers.GetOrCreateGame("CanEndSession");

			var user = Helpers.GetOrCreateUser($"CanEndSession");
			var session = sessionTracker.StartSession(game.Id, user.Id);

			// Act
			sessionTracker.EndSession(session.Id);

			// Assert
			Assert.False(sessionTracker.IsActive(session.Id));
		}

		/// <summary>
		///     Make sure timed out sessions are removed while active ones are kept
		/// </summary>
		[Fact]
		public void CanRemoveTimedOut()
		{
			// Arrange
			var timeoutSeconds = 5;
			var inactiveSessions = new List<Session>();
			var activeSessions = new List<Session>();

			var sessionTracker = new SessionTracker(new TimeSpan(0, 0, 0, timeoutSeconds), DefaultTimeoutCheckInterval);
			var game = Helpers.GetOrCreateGame("CanRemoveTimedOut");

			for (var i = 0; i < 5; i++)
			{
				var user = Helpers.GetOrCreateUser($"CanRemoveTimedOut_ShouldRemove_{i}");
				var session = sessionTracker.StartSession(game.Id, user.Id);
				inactiveSessions.Add(session);
			}

			Thread.Sleep(timeoutSeconds * 1000);

			for (var i = 0; i < 5; i++)
			{
				var user = Helpers.GetOrCreateUser($"CanRemoveTimedOut_ShouldNotRemove_{i}");
				var session = sessionTracker.StartSession(game.Id, user.Id);
				activeSessions.Add(session);
			}

			// Act
			sessionTracker
				.GetType()
				.GetMethod("RemoveTimedOut", BindingFlags.Instance | BindingFlags.NonPublic)
				.Invoke(sessionTracker, null);

			// Assert
			inactiveSessions.ForEach(s => Assert.False(sessionTracker.IsActive(s.Id)));

			activeSessions.ForEach(s => Assert.True(sessionTracker.IsActive(s.Id)));
		}

		/// <summary>
		///     Is game id/user combination added and retrievable on session started
		/// </summary>
		[Fact]
		public void CanStartSession()
		{
			// Arrange
			var sessionTracker = new SessionTracker(DefaultTimeoutCheckInterval, DefaultTimeoutCheckInterval);
			var game = Helpers.GetOrCreateGame("CanStartSession");

			var user = Helpers.GetOrCreateUser($"CanStartSession");

			// Act
			var session = sessionTracker.StartSession(game.Id, user.Id);

			// Assert
			Assert.True(sessionTracker.IsActive(session.Id));
		}

		[Fact]
		public void InactiveGetRemoved()
		{
			// Arrange
			var timeoutMilliseconds = 100;
			var game = Helpers.GetOrCreateGame("InactiveGetRemoved");
			var sessions = new List<Session>();
			var sessionTracker = new SessionTracker(new TimeSpan(0, 0, 0, 0, timeoutMilliseconds),
				new TimeSpan(0, 0, 0, 0, timeoutMilliseconds));

			for (var i = 0; i < 100; i++)
			{
				var user = Helpers.GetOrCreateUser($"CanRemoveTimedOut_ShouldRemove_{i}");
				var session = sessionTracker.StartSession(game.Id, user.Id);
				sessions.Add(session);
			}

			// Act
			Thread.Sleep(timeoutMilliseconds * 2);

			// Assert
			sessions.ForEach(s => Assert.False(sessionTracker.IsActive(s.Id)));
		}

		/// <summary>
		///     Are the sessions removed for actors that have been removed via the core controller
		/// </summary>
		[Fact]
		public void SessionRemovedOnActorRemoved()
		{
			// Arrange
			var activeSessions = new List<Session>();

			var sessionTracker = new SessionTracker(DefaultTimeoutCheckInterval, DefaultTimeoutCheckInterval);
			var game = Helpers.GetOrCreateGame("SessionRemovedOnActorRemoved");

			var removeUser = Helpers.GetOrCreateUser($"SessionRemovedOnActorRemoved_ShouldRemove");
			var removeSession = sessionTracker.StartSession(game.Id, removeUser.Id);

			for (var i = 0; i < 5; i++)
			{
				var user = Helpers.GetOrCreateUser($"SessionRemovedOnActorRemoved_ShouldNotRemove_{i}");
				var session = sessionTracker.StartSession(game.Id, user.Id);
				activeSessions.Add(session);
			}

			// Act
			ControllerLocator.UserController.Delete(removeUser.Id);

			// Assert
			Assert.False(sessionTracker.IsActive(removeSession.Id));

			activeSessions.ForEach(s => Assert.True(sessionTracker.IsActive(s.Id)));
		}

		/// <summary>
		///     Are the sessions removed for actors in a game that has been removed via the core controller
		/// </summary>
		[Fact]
		public void SessionRemovedOnGameRemoved()
		{
			// Arrange
			var inactiveSessions = new List<Session>();
			var activeSessions = new List<Session>();

			var sessionTracker = new SessionTracker(DefaultTimeoutCheckInterval, DefaultTimeoutCheckInterval);
			var removeGame = Helpers.GetOrCreateGame("SessionRemovedOnGameRemoved_Remove");
			var keepGame = Helpers.GetOrCreateGame("SessionRemovedOnGameRemoved_Keep");

			for (var i = 0; i < 5; i++)
			{
				var user = Helpers.GetOrCreateUser($"SessionRemovedOnGameRemoved_ShouldRemove_{i}");
				var session = sessionTracker.StartSession(removeGame.Id, user.Id);
				inactiveSessions.Add(session);
			}

			for (var i = 0; i < 5; i++)
			{
				var user = Helpers.GetOrCreateUser($"SessionRemovedOnGameRemoved_ShouldNotRemove_{i}");
				var session = sessionTracker.StartSession(keepGame.Id, user.Id);
				activeSessions.Add(session);
			}

			// Act
			ControllerLocator.GameController.Delete(removeGame.Id);

			// Assert
			inactiveSessions.ForEach(s => Assert.False(sessionTracker.IsActive(s.Id)));

			activeSessions.ForEach(s => Assert.True(sessionTracker.IsActive(s.Id)));
		}
	}
}