using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates log-in details.
	/// </summary>
	public class AccountRequest
	{
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		[Required]
		[StringLength(64)]
		public string Password { get; set; }

		[Required]
		public bool AutoLogin { get; set; }
	}
}
