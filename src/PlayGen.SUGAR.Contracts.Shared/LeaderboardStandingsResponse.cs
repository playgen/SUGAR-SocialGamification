namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates leaderboard standings details from the database.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     ActorId : 1,
	///     ActorName : "Name",
	///     Value : "10",
	///     Ranking : 1
	///     }
	/// </example>
	public class LeaderboardStandingsResponse : IResponse
	{
		/// <summary>
		///     The Id of the Actor.
		/// </summary>
		public int ActorId { get; set; }

		/// <summary>
		///     The name of the Actor.
		/// </summary>
		public string ActorName { get; set; }

		/// <summary>
		///     The value returned from the query for the leaderboard.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///     The actor's global ranking within that leaderboard.
		/// </summary>
		public int Ranking { get; set; }
	}
}