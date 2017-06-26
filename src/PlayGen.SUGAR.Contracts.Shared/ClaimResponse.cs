using PlayGen.SUGAR.Common.Permissions;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates role details.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     Id : 1,
	///     Token : "CreateGame",
	///     Description : "Allows for the creation of new games",
	///     ClaimScope : "Game"
	///     }
	/// </example>
	public class ClaimResponse : IResponse
	{
		/// <summary>
		///     The ID of the claim.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     The display name for the claim.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		///     A description of the claim.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///     The ClaimScope of this claim.
		/// </summary>
		public ClaimScope ClaimScope { get; set; }
	}
}