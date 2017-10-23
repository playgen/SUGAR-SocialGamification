namespace PlayGen.SUGAR.Server.Model.Interfaces
{
	public interface IRelationship
	{
		int RequestorId { get; set; }

		int AcceptorId { get; set; }
	}
}