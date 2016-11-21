using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates log-in details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Name : "User Name",
	/// Password : "Their Password",
	/// AutoLogin : true
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
		[Required]
		[StringLength(64)]
		public string Password { get; set; }

		/// <summary>
		/// Whether the user will be logged in upon registering.
		/// </summary>
		public bool AutoLogin { get; set; }
	}
}
