using System.Collections.Generic;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.Model
{
	public class User : Actor
	{
		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

		public virtual List<UserToUserRelationship> Requestors { get; set; }

		public virtual List<UserToUserRelationshipRequest> RequestRequestors { get; set; }

		public virtual List<UserToUserRelationship> Acceptors { get; set; }

		public virtual List<UserToUserRelationshipRequest> RequestAcceptors { get; set; }

		public override ActorType ActorType => ActorType.User;
	}
}
