namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and token details at log-in.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// User : {
	/// Id : 1,
	/// Name : "User Name"
	/// }
	/// }
	/// </example>
	public class AccountResponse
	{
		/// <summary>
		/// ActorResponse object containing the user details.
		/// </summary>
		public UserResponse User { get; set; }
	}
}
