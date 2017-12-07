using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
    public class Reward
    {
        public int Id { get; set; }

		public int EvaluationId { get; set; }

		public Evaluation Evaluation { get; set; }

		public string EvaluationDataKey { get; set; }

		public EvaluationDataCategory EvaluationDataCategory { get; set; }

		public EvaluationDataType EvaluationDataType { get; set; }

		public string Value { get; set; }
	}
}
