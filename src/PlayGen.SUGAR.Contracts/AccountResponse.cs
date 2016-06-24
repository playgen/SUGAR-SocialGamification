using System;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and token details at log-in.
	/// </summary>
	public class AccountResponse
	{
		public ActorResponse User { get; set; }

		/// <summary>
		/// JWT string - TEMPORARY return value
		/// </summary>
		[Obsolete("This will be set in HTTP response headers")]
		public string Token { get; set; }
	}
}
