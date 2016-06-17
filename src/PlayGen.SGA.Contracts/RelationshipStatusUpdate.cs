using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SGA.Contracts
{
    public class RelationshipStatusUpdate
    {
        public int RequestorId { get; set; }

        public int AcceptorId { get; set; }

        public bool Accepted { get; set; }
    }
}
