using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard current standings request.
	/// </summary>
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

		public DateTime? DateStart { get; set; }

		public DateTime? DateEnd { get; set; }
	}
}
