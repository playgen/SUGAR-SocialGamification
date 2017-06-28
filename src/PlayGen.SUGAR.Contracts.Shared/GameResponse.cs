namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates game details from the server.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     Id : 1,
	///     Name : "Game Name"
	///     }
	/// </example>
	public class GameResponse : Response
	{
		/// <summary>
		///     The unqiue identifier for the game.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     The display name of the game.
		/// </summary>
		public string Name { get; set; }
	}
}