using System;

namespace PlayGen.SUGAR.Core.Sessions
{
    public class Session
    {
        public int Id { get; set; }

        public int ActorId { get; set; }

        public int? GameId { get; set; }

        public DateTime LastActive { get; internal set; }
    }
}
