namespace PlayGen.SUGAR.Common.Shared
{
	/// <summary>
	/// Enum for selecting the type and sorting order of the leaderboard being created.
	/// </summary>
	public enum LeaderboardType
	{
		/// <summary>
		/// Sort by the highest EvaluationData values for a key. Only the highest is taken for each Actor.
		/// </summary>
		Highest = 0,
		/// <summary>
		/// Sort by the lowest EvaluationData values for a key. Only the lowest is taken for each Actor.
		/// </summary>
		Lowest,
		/// <summary>
		/// Sort by the highest sum of EvaluationData values for a key for an actor.
		/// </summary>
		Cumulative,
		/// <summary>
		/// Sort by the highest count of a EvaluationData key for an actor.
		/// </summary>
		Count,
		/// <summary>
		/// Sort by the earliest occurence of a EvaluationData key. Only the earliest 'DateCreation' is taken for each Actor.
		/// </summary>
		Earliest,
		/// <summary>
		/// Sort by the most recent occurence of a EvaluationData key. Only the most recent 'DateModified' is taken for each Actor.
		/// </summary>
		Latest
	}
}
