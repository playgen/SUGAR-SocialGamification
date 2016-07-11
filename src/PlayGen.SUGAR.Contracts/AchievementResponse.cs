using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates achievement/skill details returned from the server.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Token : "AN_ACHIEVEMENT_TOKEN",
	/// GameId : 1,
	/// Name : "Achievement Unlocked",
	/// Description : "Fulfil the criteria to get the reward",
	/// ActorType : "User",
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
	/// </example>
	public class AchievementResponse
	{
		/// <summary>
		/// A unique identifier used in development to reference the achievement/skill.
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The ID of the Game which this achievement/skill belongs to.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The display name for the achievement/skill.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The description of the achievement/skill.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The type of actor which this achievement/skill is intended to be completed by.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ActorType ActorType { get; set; }

		/// <summary>
		/// A list of criteria which is checked in order to see if an actor has completed the achievement/skill.
		/// </summary>
		public List<AchievementCriteria> CompletionCriteria { get; set; }

		/// <summary>
		/// A list of rewards that is provided to the actor upon completion of the achievement/skill criteria.
		/// </summary>
		public List<Reward> Reward { get; set; }
	}
}
