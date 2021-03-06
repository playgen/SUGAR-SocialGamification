﻿namespace PlayGen.SUGAR.Common
{
	/// <summary>
	/// Enum for selecting the type and sorting order of the leaderboard being created.
	/// </summary>
	public enum LeaderboardType
	{
		/// <summary>
		/// Sort by the highest EvaluationData values for a key.
		/// </summary>
		Highest = 0,
		/// <summary>
		/// Sort by the lowest EvaluationData values for a key.
		/// </summary>
		Lowest = 1,
		/// <summary>
		/// Sort by the highest sum of EvaluationData values for a key for an actor.
		/// </summary>
		Cumulative = 2,
		/// <summary>
		/// Sort by the highest count of a EvaluationData key for an actor.
		/// </summary>
		Count = 3,
		/// <summary>
		/// Sort by the earliest occurence of a EvaluationData key.
		/// </summary>
		Earliest = 4,
		/// <summary>
		/// Sort by the most recent occurence of a EvaluationData key.
		/// </summary>
		Latest = 5
	}
}
