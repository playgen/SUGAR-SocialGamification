using System.ComponentModel.DataAnnotations;

using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement or skill.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "EvaluationDataKey" : "Reward Key",
	/// "EvaluationDataCategory" : "Resource",
	/// "EvaluationDataType" : "Long",
	/// "Value" : "11"
	/// }
	/// </example>
	public class RewardCreateRequest
    {
		/// <summary>
		/// The key which will be stored in EvaluationData.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string EvaluationDataKey { get; set; }

		/// <summary>
		/// EvaluationDataCategory of the value for this EvaluationData.
		/// </summary>
		[Required]
		public EvaluationDataCategory? EvaluationDataCategory { get; set; }

		/// <summary>
		/// EvaluationDataType of the value for this EvaluationData.
		/// </summary>
		[Required]
		public EvaluationDataType? EvaluationDataType { get; set; }

		/// <summary>
		/// The value of the EvaluationData.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Value { get; set; }
	}
}