namespace PlayGen.SUGAR.Contracts.Shared
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
		public ActorResponse User { get; set; }

		public AccountResponse()
		{
		}
	}
}
