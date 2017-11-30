namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for selecting the type and sorting order of the leaderboard being created.
	/// </summary>
	public enum LeaderboardType
	{
		/// <summary>
		/// Sort by the highest GameData values for a key. Only the highest is taken for each Actor.
		/// </summary>
		Highest = 0,
		/// <summary>
		/// Sort by the lowest GameData values for a key. Only the lowest is taken for each Actor.
		/// </summary>
		Lowest = 1,
		/// <summary>
		/// Sort by the highest sum of GameData values for a key for an actor.
		/// </summary>
		Cumulative = 2,
		/// <summary>
		/// Sort by the highest count of a GameData key for an actor.
		/// </summary>
		Count = 3,
		/// <summary>
		/// Sort by the earliest occurence of a GameData key. Only the earliest 'DateCreation' is taken for each Actor.
		/// </summary>
		Earliest = 4,
		/// <summary>
		/// Sort by the most recent occurence of a GameData key. Only the most recent 'DateModified' is taken for each Actor.
		/// </summary>
		Latest = 5
	}
}
