namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates relationship details from the server.
	/// </summary>
	public class RelationshipResponse
	{
		public int RequestorId { get; set; }

		public int AcceptorId { get; set; }
	}
}
