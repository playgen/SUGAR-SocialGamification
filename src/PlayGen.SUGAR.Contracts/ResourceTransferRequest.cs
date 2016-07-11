using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details.
	/// </summary>
	public class ResourceTransferRequest
	{
		[Required]
		public int SenderActorId { get; set; }

		[Required]
		public int? RecipientActorId { get; set; }

		public int? GameId { get; set; }

		[Required]
		public long Quantity { get; set; }

		[Required]
		[StringLength(64)]
		public string Key { get; set; }
	}
}