using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard standings details from the database.
	/// </summary>
	public class LeaderboardStandingsResponse
	{
		public int ActorId { get; set; }

		public string Value { get; set; }

		public int Ranking { get; set; }
	}
}
