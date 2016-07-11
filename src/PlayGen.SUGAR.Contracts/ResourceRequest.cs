using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Data details.
	/// </summary>
	public class ResourceRequest
	{
		public int? Id { get; set; }

		/// <summary>
		/// The id of the Actor which this Resource is being ensigned to. Can be left null to ensign to the system/game.
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this Resource relates to. Can be left null to relate the Resource to the wider system.
		/// </summary>
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
		public long Quantity { get; set; }
	}
}