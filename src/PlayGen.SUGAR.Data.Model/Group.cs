using System.Collections.Generic;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Data.Model
{
	public class Group : Actor
	{
		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

		public override Common.ActorType ActorType => Common.ActorType.Group;
	}
}
