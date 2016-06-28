namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates relationship details, including updated status of the relationship.
	/// </summary>
	public class RelationshipStatusUpdate
	{
		public int RequestorId { get; set; }

		public int AcceptorId { get; set; }

		public bool Accepted { get; set; }
	}
}
