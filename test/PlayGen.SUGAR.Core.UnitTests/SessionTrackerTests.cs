using System;
using System.Collections.Generic;
using System.Threading;
using PlayGen.SUGAR.Core.Sessions;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests
{
    [Collection("Project Fixture Collection")]
    public class SessionTrackerTests
    {
        /// <summary>
        /// Is game id/user combination added and retrievable on session started
        /// </summary>
        [Fact]
        public void CanStartSession()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is game id/user combination removed and not retrievable on session ended
        /// </summary>
        [Fact]
        public void CanEndSession()
        {
            throw new NotImplementedException();
        }
      
        /// <summary>
        /// Are the sessions removed for actors that have been removed via the core controller
        /// </summary>
        [Fact]
        public void SessionRemovedOnActorRemoved()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Are the sessions removed for actors in a game that has been removed via the core controller
        /// </summary>
        [Fact]
        public void SessionRemovedOnGameRemoved()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Make sure timed out sessions are removed while active ones are kept
        /// </summary>
        [Fact]
        public void CanRemoveTimedout()
        {
            // Arrange
            var timeoutSeconds = 5;
            var inactiveSessions = new List<Session>();
            var activeSessions = new List<Session>();

            var sessionTracker = new SessionTracker(new TimeSpan(0, 0, 0, timeoutSeconds));
            var game = Helpers.GetOrCreateGame("CanRemoveTimedout");

            for (var i = 0; i < 5; i++)
            {
                var user = Helpers.GetOrCreateUser($"CanRemoveTimedout_ShouldRemove_{i}");
                var session = sessionTracker.StartSession(game.Id, user.Id);
                inactiveSessions.Add(session);
            }

            Thread.Sleep(timeoutSeconds * 1000);

            for (var i = 0; i < 5; i++)
            {
                var user = Helpers.GetOrCreateUser($"CanRemoveTimedout_ShouldNotRemove_{i}");
                var session = sessionTracker.StartSession(game.Id, user.Id);
                activeSessions.Add(session);
            }

            // Act
            sessionTracker.RemoveTimedOut();

            // Assert
            inactiveSessions.ForEach(s => Assert.False(sessionTracker.IsActive(s.Id)));

            activeSessions.ForEach(s => Assert.True(sessionTracker.IsActive(s.Id)));
        }
    }
}
