﻿using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Contracts.Shared
{
	/// <summary>
	/// Encapsulates leaderboard details returned from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Token : "THE_LEADERBOARD_TOKEN",
	/// GameId : 1,
	/// Name : "Leaderboard Name",
	/// Key : "Key",
	/// ActorType : "User",
	/// SaveDataType : "Long",
	/// CriteriaScope : "Actor",
	/// LeaderboardType : "Highest"
	/// }
	/// </example>
	public class LeaderboardResponse
	{
		/// <summary>
		/// The unique identifier used in development to reference the leaderboard.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The Id of the game which this leaderboard belongs to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The display name of the leaderboard.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The EvaluationData key which is checked against in order to create the leaderboard standings.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The type of actor which this leaderboard is intended for.
		/// </summary>
		public ActorType ActorType { get; set; }

		/// <summary>
		/// The SaveDataType of the EvaluationData being checked against.
		/// </summary>
		public SaveDataType SaveDataType { get; set; }

		/// <summary>
		/// Whether the criteria is checked against the actor or relatedactors (i.e. group members, user friends).
		/// </summary>
		public CriteriaScope CriteriaScope { get; set; }

		/// <summary>
		/// The method which collects data and sorts it for this leaderboard.
		/// </summary>
		public LeaderboardType LeaderboardType { get; set; }
	}
}