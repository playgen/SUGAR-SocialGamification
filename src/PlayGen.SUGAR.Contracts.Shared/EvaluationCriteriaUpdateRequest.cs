namespace PlayGen.SUGAR.Contracts.Shared
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
	public class EvaluationCriteriaUpdateRequest : Common.Shared.EvaluationCriteria
	{
		/// <summary>
		/// The unqiue identifier for this type.
		/// </summary>
		public int Id { get; set; }

		// todo make all fields required for contracts
	}
}