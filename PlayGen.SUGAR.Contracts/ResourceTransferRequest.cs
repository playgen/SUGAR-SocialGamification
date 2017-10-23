using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates Resource Transfer details.
	/// </summary>
	public class ResourceTransferRequest 
	{
		/// <summary>
		/// The Id of the Actor who will send this Resource. Can be left null to only give/take from the current owner.
		/// </summary>
		public int? SenderActorId { get; set; }

		/// <summary>
		/// The Id of the Actor who will receive this Resource. Can be left null to only give/take from the current owner.
		/// </summary>
		public int? RecipientActorId { get; set; }

		/// <summary>
		/// The Id of the Game which this Resource belongs to. Left null for system-wise resources.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The amount of the Resource being transferred.
		/// </summary>
		[Required]
		public long Quantity { get; set; }

		/// <summary>
		/// The key of the Resource being transferred.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Key { get; set; }
	}
}