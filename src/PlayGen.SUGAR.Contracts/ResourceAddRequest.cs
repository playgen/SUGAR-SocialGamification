using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Data details.
	/// </summary>
	public class ResourceAddRequest
	{
		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		[Required]
		[StringLength(64)]
		public string Key { get; set; }

		[Required]
		public long Quantity { get; set; }
	}
}