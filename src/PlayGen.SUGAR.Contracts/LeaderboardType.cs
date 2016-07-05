namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Enum for selecting the type and sorting order of the leaderboard being created.
	/// </summary>
	public enum LeaderboardType
	{
		Highest = 0,
		Lowest,
		Cumulative,
		Count,
		Earliest,
		Latest
	}
}
