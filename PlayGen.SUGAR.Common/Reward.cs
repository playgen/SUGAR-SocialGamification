using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement or skill.
	/// </summary>
	public class Reward
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