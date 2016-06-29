using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates savedata details.
	/// </summary>
	public class SaveDataRequest
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
