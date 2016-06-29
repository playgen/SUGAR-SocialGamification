using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement.
	/// </summary>
	public class Reward
	{
		[Required]
		public string Key { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType DataType { get; set; }

		[Required]
		public string Value { get; set; }
	}
}