using System;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests
{
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
    }
}
