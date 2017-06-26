using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class UserToUserRelationshipRequest : IRelationship
	{
		public User Requestor { get; set; }

		public User Acceptor { get; set; }
		public int RequestorId { get; set; }

		public int AcceptorId { get; set; }
	}
}