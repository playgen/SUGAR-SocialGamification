namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates actorclaim details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// ActorId : 1,
	/// RoleId : 1,
	/// EntityId : 1
	/// }
	/// </example>
	public class ActorClaimResponse
	{
		/// <summary>
		/// The unqiue identifier for the actorclaim.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The ID of the actor related to this actorclaim.
		/// </summary>
		public int ActorId { get; set; }

		/// <summary>
		/// The ID of the claim related to this actorclaim.
		/// </summary>
		public int ClaimId { get; set; }

		/// <summary>
		/// The ID of the entity (game, actor etc) related to this actorclaim.
		/// </summary>
		public int? EntityId { get; set; }
	}
}