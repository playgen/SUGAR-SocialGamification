using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates achievement details returned from the server.
	/// </summary>
	public class AchievementResponse
	{
		public int Id { get; set; }

		public int? GameId { get; set; }

		public string Name { get; set; }

		public ActorType ActorType { get; set; }

		public List<AchievementCriteria> CompletionCriteria { get; set; }

		public List<Reward> Reward { get; set; }
	}
}
