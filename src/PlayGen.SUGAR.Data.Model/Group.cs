using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model.Interfaces;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Data.Model
{
	public class Group : Actor
	{
		public string Name { get; set; }

		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

		public override ActorType ActorType => ActorType.Group;
	}
}
