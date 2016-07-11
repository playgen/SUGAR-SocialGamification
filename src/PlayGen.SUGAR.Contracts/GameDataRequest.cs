using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates GameData details.
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
	public class GameDataRequest
	{
		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		[Required]
		[StringLength(64)]
		public string Key { get; set; }

		[Required]
		[StringLength(64)]
		public string Value { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType GameDataType { get; set; }
	}
}
