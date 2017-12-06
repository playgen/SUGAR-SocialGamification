using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement or skill.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Key : "Reward Key",
	/// DataType : "Float",
	/// Value : "10.5"
	/// }
	/// </example>
	public class RewardResponse
	{
		/// <summary>
		/// The unqiue identifier for this type.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The key which will be stored in EvaluationData.
		/// </summary>
		public string EvaluationDataKey { get; set; }

		/// <summary>
		/// EvaluationDataCategory of the value for this EvaluationData.
		/// </summary>
		public EvaluationDataCategory? EvaluationDataCategory { get; set; }

		/// <summary>
		/// EvaluationDataType of the value for this EvaluationData.
		/// </summary>
		public EvaluationDataType? EvaluationDataType { get; set; }

		/// <summary>
		/// The value of the EvaluationData.
		/// </summary>
		public string Value { get; set; }
	}
}