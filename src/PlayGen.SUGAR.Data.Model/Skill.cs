using System.Collections.Generic;

namespace PlayGen.SUGAR.Data.Model
{
	public class Skill
    {
		public int GameId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Common.Shared.ActorType ActorType { get; set; }

		public string Token { get; set; }

		public virtual List<CompletionCriteria> CompletionCriterias { get; set; }

		public virtual List<Reward> Rewards { get; set; }
	}
}
