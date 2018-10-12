namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates relationship details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "RequestorId" : 1,
	/// "AcceptorId" : 2
	/// }
	/// </example>
	public class RelationshipResponse
	{
		/// <summary>
		/// The Id of the requesting actor.
		/// </summary>
		public int RequestorId { get; set; }

		/// <summary>
		/// The Id of the receiving actor.
		/// </summary>
		public int AcceptorId { get; set; }
	}
}
