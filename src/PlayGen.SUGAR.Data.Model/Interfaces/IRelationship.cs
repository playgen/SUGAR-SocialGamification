namespace PlayGen.SUGAR.Data.Model.Interfaces
{
	public interface IRelationship
	{
		int RequestorId { get; set; }

		int AcceptorId { get; set; }
	}
}