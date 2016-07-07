using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates achievement details returned from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Id : 1,
	/// GameId : 1,
	/// Name : "Achievement Unlocked",
	/// Description : "Fulfil the criteria to get the reward",
	/// ActorType : "User",
	/// Token : "Achievement_Token",
	/// CompletionCriteria : [{
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
	/// }
	/// </example>
	public class AchievementResponse
	{
		public int Id { get; set; }

		public int? GameId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		public string Token { get; set; }

		public List<AchievementCriteria> CompletionCriteria { get; set; }

		public List<Reward> Reward { get; set; }
	}
}
