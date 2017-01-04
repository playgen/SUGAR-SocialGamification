using System.Collections.Generic;

namespace PlayGen.SUGAR.Data.Model
{
	public class Group : Actor
	{
		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

		public override Common.Shared.ActorType ActorType => Common.Shared.ActorType.Group;
	}
}
