using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.Model
{
	public abstract class Actor
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<ActorDetails> Details { get; set; }

		public abstract ActorType ActorType { get; }
	}
}
