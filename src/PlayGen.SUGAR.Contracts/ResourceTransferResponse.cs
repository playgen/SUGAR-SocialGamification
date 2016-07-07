namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details from the server.
	/// </summary>
    public class ResourceTransferResponse
    {
		public ResourceResponse FromResource { get; set; }

		public ResourceResponse ToResource { get; set; }
	}
}
