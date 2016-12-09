using System;

namespace PlayGen.SUGAR.Data.Model
{
    public class Match
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Ended { get; set; }
    }
}
