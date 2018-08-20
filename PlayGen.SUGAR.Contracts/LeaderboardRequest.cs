using System.ComponentModel.DataAnnotations;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "Token" : "THE_LEADERBOARD_TOKEN",
	/// "GameId" : 1,
	/// "Name" : "Leaderboard Name",
	/// "EvaluationDataCategory" : "GameData",
	/// "Key" : "Key",
	/// "ActorType" : "User",
	/// "EvaluationDataType" : "Long",
	/// "CriteriaScope" : "Actor",
	/// "LeaderboardType" : "Highest"
	/// }
	/// </example>
	public class LeaderboardRequest
    {
		/// <summary>
		/// A unique identifier used in development to reference the leaderboard.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Token { get; set; }

		/// <summary>
		/// The Id of the Game which this leaderboards belongs to. This Id is also used when gathering EvaluationData using the Key provided.
		/// </summary>
		[Required]
		public int? GameId { get; set; }

		/// <summary>
		/// The display name of the leaderboard.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

        /// <summary>
		/// The EvaluationDataCategory of the EvaluationData being checked against.
		/// </summary>
		[Required]
        public EvaluationDataCategory? EvaluationDataCategory { get; set; }

        /// <summary>
        /// The EvaluationData key which is checked against in order to create the leaderboard standings.
        /// </summary>
        [Required]
		[StringLength(64)]
		public string Key { get; set; }

		/// <summary>
		/// The type of actor which this leaderboard is intended for.
		/// </summary>
		[Required]
		public ActorType? ActorType { get; set; }

		/// <summary>
		/// The EvaluationDataType of the EvaluationData being checked against.
		/// </summary>
		[Required]
		public EvaluationDataType? EvaluationDataType { get; set; }

		/// <summary>
		/// Whether the criteria will be checked against the actor or relatedactors (i.e. group members, user friends).
		/// </summary>
		[Required]
		public CriteriaScope? CriteriaScope { get; set; }

		/// <summary>
		/// The method which data will be collected and sorted for this leaderboard.
		/// </summary>
		[Required]
		public LeaderboardType? LeaderboardType { get; set; }
	}
}
