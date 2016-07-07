using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details.
	/// </summary>
    public class ResourceTransferRequest
    {
		[Required]
		public int ResourceId { get; set; }
		
		public int? RecipientId { get; set; }

		public int? GameId { get; set; }

		[Required]
		public long Quantity { get; set; }
    }
}