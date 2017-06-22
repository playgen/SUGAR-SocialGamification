namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and group details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// Name : "Actor Name"
	/// }
	/// </example>
	public class ActorResponse
	{
		/// <summary>
		/// The unqiue identifier for the user/group.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The display name of the user/group.
		/// </summary>
		public string Name { get; set; }

		public ActorResponse()
		{
		}
	}
}