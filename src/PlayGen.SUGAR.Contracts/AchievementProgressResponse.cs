namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates current progress to completing an achievement.
	/// </summary>
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
