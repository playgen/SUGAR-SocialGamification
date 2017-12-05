using System.ComponentModel.DataAnnotations;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement or skill.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Key : "GameData Key",
	/// DataType : "String",
	/// CriteriaQueryType : "Any",
	/// ComparisonType : "Equals",
	/// Scope : "Actor",
	/// Value : "GameData Key Value"
	/// }
	/// </example>
	public class EvaluationCriteriaUpdateRequest : EvaluationCriteria
	{
		/// <summary>
		/// The unqiue identifier for this type.
		/// </summary>
		[Required]
		public int? Id { get; set; }

		// todo make all fields required for contracts
	}
}