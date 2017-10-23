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
	public class RewardUpdateRequest : Reward
	{
		/// <summary>
		/// The unqiue identifier for this type.
		/// </summary>
		public int Id { get; set; }

		// todo make all fields required for contract
	}
}