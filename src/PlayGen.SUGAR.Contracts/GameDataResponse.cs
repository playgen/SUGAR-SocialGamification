using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates GameData details from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// ActorId : 1,
	/// GameId : 1,
	/// Key : "Data Key",
	/// Value : "10",
	/// GameDataType : "Long"
	/// }
	/// </example>
	public class GameDataResponse
	{
		/// <summary>
		/// The id of the Actor which this GameData relates to.
		/// </summary>
		public int? ActorId { get; set; }

		/// <summary>
		/// The id of the Game which this GameData relates to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The identifier of the data.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// The value of the data.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// The type of the value for this data.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }
	}
}
