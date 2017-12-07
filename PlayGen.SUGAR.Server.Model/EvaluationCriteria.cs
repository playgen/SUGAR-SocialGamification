using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
	public class EvaluationCriteria
	{
		public int Id { get; set; }

		public int EvaluationId { get; set; }

		public Evaluation Evaluation { get; set; }

		public string EvaluationDataKey { get; set; }

		public EvaluationDataCategory EvaluationDataCategory { get; set; }

		public EvaluationDataType EvaluationDataType { get; set; }

		public CriteriaQueryType CriteriaQueryType { get; set; }

		public ComparisonType ComparisonType { get; set; }

		public CriteriaScope Scope { get; set; }

		public string Value { get; set; }
	}
}