using System;

namespace PlayGen.SUGAR.Contracts.Shared
{
    public class MatchResponse
    {
        public int Id { get; set; }

        public GameResponse Game { get; set; }

        public UserResponse Creator { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Ended { get; set; }
    }
}
