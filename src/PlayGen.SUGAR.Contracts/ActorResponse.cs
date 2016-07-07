namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and group details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// Name : "Name"
	/// }
	/// </example>
	public class ActorResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}