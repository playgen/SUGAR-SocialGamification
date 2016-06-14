using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class UserToUserRelationship : IRecord, IRelationship
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public User Requestor { get; set; }

        public int AcceptorId { get; set; }

        public User Acceptor { get; set; }
    }
}
