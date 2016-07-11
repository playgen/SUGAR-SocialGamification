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
	/// Token : "THE_LEADERBOARD_TOKEN",
	/// GameId : 1,
	/// Name : "Leaderboard Name",
	/// Key : "Key",
	/// ActorType : "User",
	/// GameDataType : "Long",
	/// CriteriaScope : "Actor",
	/// LeaderboardType : "Highest"
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
		/// The Id of the Game which this leaderboards belongs to. Can be left null to relate this leaderboard to the system rather than a game.
		/// This Id is also used when gathering GameData using the Key provided.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The display name of the leaderboard.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		/// <summary>
		/// The GameData key which is checked against in order to create the leaderboard standings.
		/// </summary>
		[StringLength(64)]
		public string Key { get; set; }

		/// <summary>
		/// The type of actor which this leaderboard is intended for.
		/// </summary>
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		/// <summary>
		/// The GameDataType of the GameData being checked against.
		/// </summary>
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }

		/// <summary>
		/// Whether the criteria will be checked against the actor or relatedactors (i.e. group members, user friends).
		/// </summary>
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope CriteriaScope { get; set; }

		/// <summary>
		/// The method which data will be collected and sorted for this leaderboard.
		/// </summary>
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public LeaderboardType LeaderboardType { get; set; }
	}
}
