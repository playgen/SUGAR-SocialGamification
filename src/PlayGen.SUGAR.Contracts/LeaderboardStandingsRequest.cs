using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
	/// LeaderboardId : 1,
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
		[Required]
		public int LeaderboardId { get; set; }

		public int? ActorId { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public LeaderboardFilterType LeaderboardFilterType { get; set; }

		[Required]
		public int Limit { get; set; }

		[Required]
		public int Offset { get; set; }

		public DateTime DateStart { get; set; }

		public DateTime DateEnd { get; set; }
	}
}
