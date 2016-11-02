using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Data.Model
{
	public abstract class Actor
	{
		public int Id { get; set; }

		public abstract ActorType ActorType { get; }
	}
}