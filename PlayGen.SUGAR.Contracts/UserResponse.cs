namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// Name : "Actor Name"
	/// }
	/// </example>
	public class UserResponse : ActorResponse
	{
		/// <summary>
		/// The number of friends this user has.
		/// </summary>
		public int FriendCount { get; set; }

		/// <summary>
		/// The number of groups this user is a member of
		/// </summary>
		public int GroupCount { get; set; }
	}
}