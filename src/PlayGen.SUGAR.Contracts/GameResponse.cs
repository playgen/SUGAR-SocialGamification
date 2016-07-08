namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates game details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// Name : "Game Name"
	/// }
	/// </example>
	public class GameResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
