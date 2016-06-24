namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates current progress to completing an achievement.
	/// </summary>
	public class AchievementProgressResponse
	{
		public ActorResponse Actor { get; set; }

		public string Name { get; set; }

		public float Progress { get; set; }
	}
}
