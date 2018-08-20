using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Data details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "ActorId" : 1,
	/// "GameId" : 1,
	/// "Key" : "Resource Key",
	/// "Quantity" : 20
	/// }
	/// </example>
	public class ResourceAddRequest
	{
		/// <summary>
		/// The id of the Actor which this Resource is being ensigned to.
		/// </summary>
		[Required]
		public int? ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this Resource relates to.
		/// </summary>
		[Required]
		public int? GameId { get; set; }

		/// <summary>
		/// The identifier/name of the Resource.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Key { get; set; }

		/// <summary>
		/// The value of the Resource.
		/// </summary>
		[Required]
		public long? Quantity { get; set; }
	}
}