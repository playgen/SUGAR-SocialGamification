namespace PlayGen.SUGAR.Data.Model
{
	public class GroupData : GameData
	{
		public int GroupId { get; set; }

		public virtual Group Group { get; set; }

		public override int ActorId => GroupId;
	}
}
