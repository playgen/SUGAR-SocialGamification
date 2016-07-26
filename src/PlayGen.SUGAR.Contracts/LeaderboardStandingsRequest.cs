using System;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard current standings request.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// LeaderboardToken : "AN_ACHIEVEMENT_TOKEN",
	/// GameId : 1,
	/// ActorId : 1,
	/// LeaderboardFilterType : "Near",
	/// Limit : 10,
	/// Offset : 0,
	/// DateStart : "2016-01-01 00:00:00",
	/// DateEnd : "2016-12-31 23:59:59"
	/// }
	/// </example>
	public class LeaderboardStandingsRequest
	{
		/// <summary>
		/// The Token of the Leaderboard which the standings are being gathered for.
		/// </summary>
		[Required]
		public string LeaderboardToken { get; set; }

		/// <summary>
		/// The Id of the Game the leaderboard. Can be left null for system-wide leaderboards.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The Id of an Actor. Required for getting standings for Near, Friends (user only) and Group Members (group only)
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		/// The filter for what standings will be returned.
		/// </summary>
		[Required]
		public LeaderboardFilterType LeaderboardFilterType { get; set; }

		/// <summary>
		/// The maximum number of results which will be returned.
		/// </summary>
		[Required]
		public int PageLimit { get; set; }

		/// <summary>
		/// The set of results which will be returned based on the limit.
		/// 0 returns the first set of results for Top, Friends and Group Member and the nearest range for Near.
		/// </summary>
		[Required]
		public int PageOffset { get; set; }

		/// <summary>
		/// The earliest point which data should be collected from.
		/// Can be left null to set no earliest point.
		/// </summary>
		public DateTime DateStart { get; set; }

		/// <summary>
		/// The latest point which data should be collected from.
		/// Can be left null to gather all data up to the present time.
		/// </summary>
		public DateTime DateEnd { get; set; }
	}
}
