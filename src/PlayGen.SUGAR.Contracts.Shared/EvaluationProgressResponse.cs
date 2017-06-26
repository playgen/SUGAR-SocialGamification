using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	///     Encapsulates current progress to completing an achievement.
	/// </summary>
	/// <example>
	///     JSON
	///     {
	///     Actor : [{
	///     Id : 1,
	///     Name : "Name"
	///     }],
	///     Name : "Achievement Unlocked",
	///     Progress : 0
	///     }
	/// </example>
	public class EvaluationProgressResponse : IResponse
	{
		/// <summary>
		///     The details of the actor whose progress was being checked.
		/// </summary>
		public ActorResponse Actor { get; set; }

		/// <summary>
		///     The name of the achievement/skill which progress was being checked for.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Progress of current achievement/skill [0 to 1].
		/// </summary>
		public float Progress { get; set; }

		/// <summary>
		///     Skill or Achievement
		/// </summary>
		public EvaluationType Type { get; set; }
	}
}