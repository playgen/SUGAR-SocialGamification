namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates Resource Transfer details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "FromResource" : {
	/// "ActorId" : 1,
	/// "GameId" : 1,
	/// "Key" : "Resource Key",
	/// "Quantity" : 0,
	/// "DateCreated" : "2018-08-12T16:32:29.482146",
	/// "DateModified" : "2018-08-12T16:42:29.482146"
	/// },
	/// "ToResource" : {
	/// "ActorId" : 2,
	/// "GameId" : 1,
	/// "Key" : "Resource Key",
	/// "Quantity" : 20,
	/// "DateCreated" : "2018-08-12T16:32:29.482146",
	/// "DateModified" : "2018-08-12T16:42:29.482146"
	/// },
	/// }
	/// </example>
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
