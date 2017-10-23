using System;

namespace PlayGen.SUGAR.Core.Sessions
{
    public class Session
    {
        public long Id { get; private set; }

        public int ActorId { get; private set; }

        public int? GameId { get; private set; }

        public DateTime LastActive { get; internal set; }

        private static long _idCounter;

        public Session(int? gameId, int actorId)
        {
            Id = ++_idCounter;
            ActorId = actorId;
            GameId = gameId;
            LastActive = DateTime.UtcNow;
        }
    }
}
