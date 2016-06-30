using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard details returned from the server.
	/// </summary>
	public class LeaderboardResponse
	{
		public int Id { get; set; }

		public int? GameId { get; set; }

		public string Name { get; set; }

		public string Token { get; set; }

		public string Key { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope CriteriaScope { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public LeaderboardType LeaderboardType { get; set; }
	}
}