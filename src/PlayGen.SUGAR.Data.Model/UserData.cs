namespace PlayGen.SUGAR.Data.Model
{
	public class UserData : GameData
	{
		public int UserId { get; set; }

		public virtual User User { get; set; }

		public override int ActorId => UserId;
	}
}
