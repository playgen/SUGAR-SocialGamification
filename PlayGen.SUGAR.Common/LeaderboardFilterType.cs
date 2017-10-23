namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for selecting subsection of users for current standings of a leaderboard.
	/// </summary>
	public enum LeaderboardFilterType
	{
		/// <summary>
		/// Provides standings based off the global ranking of the leaderboard.
		/// </summary>
		Top = 0,
		/// <summary>
		/// Provides standings in relation to the actorId provided. 
		/// </summary>
		Near,
		/// <summary>
		/// Provides only the standings of those who are friends of the actorId provided.
		/// </summary>
		Friends,
		/// <summary>
		/// Provides only the standings of those who are members of the actorId provided.
		/// </summary>
		GroupMembers
	}
}