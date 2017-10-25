using System.Collections.Generic;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
    public abstract class Evaluation
    {
        public int Id { get; set; }

        public abstract EvaluationType EvaluationType { get; }

        public int? GameId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ActorType ActorType { get; set; }

        public string Token { get; set; }

        public virtual List<EvaluationCriteria> EvaluationCriterias { get; set; }

        public virtual List<Reward> Rewards { get; set; }
    }
}
