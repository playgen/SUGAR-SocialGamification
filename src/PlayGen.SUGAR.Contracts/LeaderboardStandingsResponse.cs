namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard standings details from the database.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// ActorId : 1,
	/// ActorName : "Name",
	/// Value : "10",
	/// Ranking : 1
	/// }
	/// </example>
	public class LeaderboardStandingsResponse
	{
		public int ActorId { get; set; }

		public string ActorName { get; set; }

		public string Value { get; set; }

		public int Ranking { get; set; }
	}
}
