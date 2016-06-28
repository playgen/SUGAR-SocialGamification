using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class User : Actor
	{
		public string Name { get; set; }

		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

		public virtual List<UserToUserRelationship> Requestors { get; set; }

		public virtual List<UserToUserRelationshipRequest> RequestRequestors { get; set; }

		public virtual List<UserToUserRelationship> Acceptors { get; set; }

		public virtual List<UserToUserRelationshipRequest> RequestAcceptors { get; set; }


		public override ActorType ActorType => ActorType.User;
	}
}
