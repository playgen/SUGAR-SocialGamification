using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates the reward given for completing an achievement.
	/// </summary>
	public class Reward
	{
		public string Key { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType DataType { get; set; }

		public string Value { get; set; }
	}
}