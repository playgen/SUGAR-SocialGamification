using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates log-in details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Name : "User Name",
	/// Password : "Their Password",
	/// SourceToken : "SUGAR"
	/// }
	/// </example>
	public class AccountRequest
	{
		/// <summary>
		/// The user's log-in name.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		/// <summary>
		/// The user's password.
		/// </summary>
		[StringLength(64)]
		public string Password { get; set; }

		/// <summary>
		/// The source from which the user is trying to log-in.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string SourceToken { get; set; }

		/// <summary>
		/// Whether the reqyest should return a login token for use in later logins
		/// </summary>
		public bool IssueLoginToken { get; set; }
	}
}
