using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Data.Model
{
    public class Match
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual List<EvaluationData> Data { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Ended { get; set; }
    }
}
