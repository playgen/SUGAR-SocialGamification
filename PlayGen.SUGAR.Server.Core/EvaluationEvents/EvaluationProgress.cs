using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
    public class EvaluationProgress
    {
        public Actor Actor { get; set; }

        public Evaluation Evaluation { get; set; }

        public float Progress { get; set; }
    }
}
