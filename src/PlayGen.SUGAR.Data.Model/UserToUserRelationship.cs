using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class UserToUserRelationship : IRecord, IRelationship
	{
		public int Id { get; set; }

		public int RequestorId { get; set; }

		public User Requestor { get; set; }

		public int AcceptorId { get; set; }

		public User Acceptor { get; set; }
	}
}
