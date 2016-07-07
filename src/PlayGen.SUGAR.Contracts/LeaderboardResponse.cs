using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates leaderboard details returned from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
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