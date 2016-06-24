using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class Group : IRecord
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public virtual List<GroupData> GroupDatas { get; set; }

		public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

		public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }
	}
}
