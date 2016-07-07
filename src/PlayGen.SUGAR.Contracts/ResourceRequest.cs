using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Data details.
	/// </summary>
	public class ResourceRequest
    {
		[Required]
		public int? ActorId { get; set; }

		[Required]
		public int? GameId { get; set; }

		[Required]
		[StringLength(64)]
		public string Key { get; set; }

		[Required]
		public long Quantity { get; set; }
    }
}