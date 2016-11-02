using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates relationship details, including updated status of the relationship.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// RequestorId : 1,
	/// AcceptorId : 2,
	/// Accepted : true
	/// }
	/// </example>
	public class RelationshipStatusUpdate
	{
		/// <summary>
		/// The Id of the requesting actor.
		/// </summary>
		[Required]
		public int RequestorId { get; set; }

		/// <summary>
		/// The Id of the receiving actor.
		/// </summary>
		[Required]
		public int AcceptorId { get; set; }

		/// <summary>
		/// Whether the request was accepted or declined.
		/// </summary>
		[Required]
		public bool Accepted { get; set; }
	}
}
