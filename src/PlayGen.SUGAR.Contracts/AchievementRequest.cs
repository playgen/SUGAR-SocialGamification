using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates achievement details.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// GameId : 1,
	/// Name : "Achievement Unlocked",
	/// Description : "Fulfil the criteria to get the reward",
	/// ActorType : "User",
	/// Token : "Achievement_Token",
	/// AchievementCriteria : [{
	/// Key : "Criteria Key",
	/// DataType : "Long",
	/// ComparisonType : "Equals",
	/// Scope : "Actor",
	/// Value : "5"
	/// }],
	/// Reward : [{
	/// Key : "Reward Key",
	/// DataType : "Float",
	/// Value : "10.5"
	/// }]
	/// }
	/// </example>
	public class AchievementRequest
	{
		public int? GameId { get; set; }

		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		[Required]
		public string Token { get; set; }

		[Required]
		public List<AchievementCriteria> CompletionCriteria { get; set; }

		public List<Reward> Reward { get; set; }
	}
}
