using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement.
	/// </summary>
	public class AchievementCriteria
	{
		[Required]
		public string Key { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public GameDataType DataType { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public ComparisonType ComparisonType { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public CriteriaScope Scope { get; set; }

		[Required]
		public string Value { get; set; }
	}
}
