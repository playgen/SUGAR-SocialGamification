using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.Model
{
	public class Achievement
	{
		public int GameId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ActorType ActorType { get; set; }

		public string Token { get; set; }

		public virtual List<CompletionCriteria> CompletionCriterias { get; set; }

		public virtual List<Reward> Rewards { get; set; }
	}
}
