using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement.
	/// </summary>
	public class AchievementCriteria
	{
		public string Key { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType DataType { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ComparisonType ComparisonType { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope Scope { get; set; }

		public string Value { get; set; }
	}
}
