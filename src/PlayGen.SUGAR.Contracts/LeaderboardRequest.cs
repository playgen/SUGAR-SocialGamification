using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// GameId : 1,
	/// Name : "Leaderboard Name",
	/// Token : "Leaderboard_Name",
	/// Key : "Key",
	/// ActorType : "User",
	/// GameDataType : "Long",
	/// CriteriaScope : "Actor",
	/// LeaderboardType : "Highest"
	/// }
	/// </example>
	public class LeaderboardRequest
	{
		public int? GameId { get; set; }

		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		[Required]
		[StringLength(64)]
		public string Token { get; set; }

		[StringLength(64)]
		public string Key { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope CriteriaScope { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public LeaderboardType LeaderboardType { get; set; }
	}
}
