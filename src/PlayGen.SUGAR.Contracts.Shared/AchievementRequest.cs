using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates achievement/skill details.
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
	/// CriteriaQueryType : "Any",
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
		/// <summary>
		/// A unique identifier used in development to reference the achievement/skill.
		/// </summary>
		[Required]
		public string Token { get; set; }

		/// <summary>
		/// The ID of the Game which this achievement/skill should belong to. Can be left null to make the achievement/skill system-wide.
		/// </summary>
		public int? GameId { get; set; }

		/// <summary>
		/// The display name for the achievement/skill.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

		/// <summary>
		/// The description of the achievement/skill.
		/// </summary>
		[StringLength(256)]
		public string Description { get; set; }

		/// <summary>
		/// The type of actor which this achievement/skill is intended to be completed by.
		/// </summary>
		[Required]
		public ActorType ActorType { get; set; }

		/// <summary>
		/// A list of criteria which will be checked in order to see if an actor has completed the achievement/skill.
		/// Must contain at least one criteria.
		/// </summary>
		[Required]
		public List<AchievementCriteria> CompletionCriteria { get; set; }

		/// <summary>
		/// A list of rewards that will be provided to the actor upon completion of the achievement/skill criteria.
		/// An achievement does not need to contain a reward.
		/// </summary>
		public List<Reward> Reward { get; set; }
	}
}
