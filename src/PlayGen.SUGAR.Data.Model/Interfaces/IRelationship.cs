namespace PlayGen.SUGAR.Data.Model.Interfaces
{
	public interface IRelationship
	{
		int Id { get; set; }

		int RequestorId { get; set; }

		int AcceptorId { get; set; }
	}
}