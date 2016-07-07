namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates resource details from the server.
	/// </summary>
    public class ResourceResponse
    {
		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		public string Key { get; set; }

		public long Quantity { get; set; }
    }
}
