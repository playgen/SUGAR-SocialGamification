namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details from the server.
	/// </summary>
	public class ResourceChangeResponse
	{
		/// <summary>
		/// The new details of the Resource being Added to
		/// </summary>
		public ResourceResponse Resource { get; set; }
	}
}