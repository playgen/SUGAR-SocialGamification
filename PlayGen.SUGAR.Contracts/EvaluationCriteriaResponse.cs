using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement or skill.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Key : "EvaluationData Key",
	/// DataType : "String",
	/// CriteriaQueryType : "Any",
	/// ComparisonType : "Equals",
	/// Scope : "Actor",
	/// Value : "EvaluationData Key Value"
	/// }
	/// </example>
	public class EvaluationCriteriaResponse
	{
		public int Id { get; set; }

		/// <summary>
		/// The key which will be queried against to check if criteria is met.
		/// </summary>
		public string EvaluationDataKey { get; set; }

		/// <summary>
		/// EvaluationDataCategory of the key which is being queried.
		/// </summary>
		public EvaluationDataCategory EvaluationDataCategory { get; set; }

		/// <summary>
		/// EvaluationDataType of the key which is being queried.
		/// </summary>
		public EvaluationDataType EvaluationDataType { get; set; }

		/// <summary>
		/// Which stored EvaluationData will be queried.
		/// </summary>
		public CriteriaQueryType CriteriaQueryType { get; set; }

		/// <summary>
		/// How the target value and the actual value will be compared.
		/// </summary>
		public ComparisonType ComparisonType { get; set; }

		/// <summary>
		/// Whether the criteria will be checked against the actor or related actors (i.e. group members, user friends).
		/// </summary>
		public CriteriaScope Scope { get; set; }

		/// <summary>
		/// The value which will compared against in order to see if the criteria has been met.
		/// </summary>
		public string Value { get; set; }
	}
}