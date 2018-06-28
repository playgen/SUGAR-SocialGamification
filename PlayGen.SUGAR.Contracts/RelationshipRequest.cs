using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates relationship details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// RequestorId : 1,
	/// AcceptorId : 2,
	/// AutoAccept : true
	/// }
	/// </example>
	public class RelationshipRequest
	{
		/// <summary>
		/// The Id of the requesting actor.
		/// </summary>
		[Required]
		public int? RequestorId { get; set; }

		/// <summary>
		/// The Id of the receiving actor.
		/// </summary>
		[Required]
		public int? AcceptorId { get; set; }

		/// <summary>
		/// Whether the request should be automatically accepted.
		/// </summary>
		[Required]
		public bool AutoAccept { get; set; }
	}
}
