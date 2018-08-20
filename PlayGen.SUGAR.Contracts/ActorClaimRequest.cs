using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates actorclaim details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "ActorId" : 1,
	/// "ClaimId" : 1,
	/// "EntityId" : 1
	/// }
	/// </example>
	public class ActorClaimRequest
	{
		/// <summary>
		/// The ID of the actor related to this actorclaim.
		/// </summary>
		[Required]
		public int? ActorId { get; set; }

		/// <summary>
		/// The ID of the claim related to this actorclaim.
		/// </summary>
		[Required]
		public int? ClaimId { get; set; }

		/// <summary>
		/// The ID of the entity (game, actor etc) related to this actorclaim.
		/// </summary>
		[Required]
		public int? EntityId { get; set; }
	}
}