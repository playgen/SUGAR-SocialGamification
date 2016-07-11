namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details from the server.
	/// </summary>
	public class ResourceTransferResponse
	{
		/// <summary>
		/// The new details of the Resource being transferred from.
		/// </summary>
		public ResourceResponse FromResource { get; set; }

		/// <summary>
		/// The new details of the Resource being transferred to.
		/// </summary>
		public ResourceResponse ToResource { get; set; }
	}
}
