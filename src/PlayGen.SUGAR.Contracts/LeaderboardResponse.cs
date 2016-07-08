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
	/// Token : "THE_LEADERBOARD_TOKEN",
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

		/// <summary>
		/// The Id of the game which this leaderboard belongs to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The display name of the leaderboard.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The unique identifier used in development to reference the leaderboard.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The GameData key which is checked against in order to create the leaderboard standings.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The type of actor which this leaderboard is intended for.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		/// <summary>
		/// The GameDataType of the GameData being checked against.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }

		/// <summary>
		/// Whether the criteria is checked against the actor or relatedactors (i.e. group members, user friends).
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope CriteriaScope { get; set; }

		/// <summary>
		/// The method which collects data and sorts it for this leaderboard.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public LeaderboardType LeaderboardType { get; set; }
	}
}