using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details.
	/// </summary>
	public class ResourceTransferRequest
	{
		/// <summary>
		/// The Id of the Resource being transferred.
		/// </summary>
		[Required]
		public int ResourceId { get; set; }

		/// <summary>
		/// The Id of the Actor who will receive this Resource. Can be left null to only give/take from the current owner.
		/// </summary>
		public int? RecipientId { get; set; }

		/// <summary>
		/// The Id of the Game which this Resource belongs to. Left null for system-wise resources.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The amount of the Resource being transferred.
		/// </summary>
		[Required]
		public long Quantity { get; set; }
	}
}