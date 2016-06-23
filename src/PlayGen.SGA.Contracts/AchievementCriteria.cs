using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SGA.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement.
	/// </summary>
	public class AchievementCriteria
	{
		public string Key { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public DataType DataType { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ComparisonType ComparisonType { get; set; }

		public string Value { get; set; }
	}
}
