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
		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }
	}
}
