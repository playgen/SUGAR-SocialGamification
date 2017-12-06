using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
    public class EvaluationProgress
    {
        // todo should probably store token rather than name

        public Actor Actor { get; set; }

        public string Name { get; set; }

        public float Progress { get; set; }
    }
}
