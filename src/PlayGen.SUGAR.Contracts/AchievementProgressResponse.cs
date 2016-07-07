namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates current progress to completing an achievement.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// Actor : [{
	/// Id : 1,
	/// Name : "Name"
	/// }],
	/// Name : "Name",
	/// Progress : 0
	/// }
	/// </example>
	public class AchievementProgressResponse
	{
		public ActorResponse Actor { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// Progress of current achievement [0 to 1]
		/// </summary>
		public float Progress { get; set; }
	}
}
