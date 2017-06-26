using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class UserToGroupRelationship : IRelationship
	{
		public User Requestor { get; set; }

		public Group Acceptor { get; set; }
		public int RequestorId { get; set; }

		public int AcceptorId { get; set; }
	}
}