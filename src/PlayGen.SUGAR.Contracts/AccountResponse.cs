using System;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates user and token details at log-in.
	/// </summary>
	public class AccountResponse
	{
		public ActorResponse User { get; set; }
	}
}
