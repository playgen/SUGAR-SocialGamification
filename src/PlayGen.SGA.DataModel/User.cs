using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class User : IRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<UserData> UserDatas { get; set; }

        public virtual List<UserToGroupRelationship> UserToGroupRelationships { get; set; }

        public virtual List<UserToGroupRelationshipRequest> UserToGroupRelationshipRequests { get; set; }

        public virtual List<UserToUserRelationship> Requestors { get; set; }

        public virtual List<UserToUserRelationshipRequest> RequestRequestors { get; set; }

        public virtual List<UserToUserRelationship> Acceptors { get; set; }

        public virtual List<UserToUserRelationshipRequest> RequestAcceptors { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
