namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates relationship details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// RequestorId : 1,
	/// AcceptorId : 2
	/// }
	/// </example>
	public class RelationshipResponse
	{
		public int RequestorId { get; set; }

		public int AcceptorId { get; set; }
	}
}
