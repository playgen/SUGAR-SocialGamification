namespace PlayGen.SUGAR.Data.Model
{
	public abstract class Actor
	{
		public int Id { get; set; }

		public abstract Common.Shared.ActorType ActorType { get; }
	}
}