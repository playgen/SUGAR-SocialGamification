using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
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
