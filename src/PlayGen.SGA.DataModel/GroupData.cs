using PlayGen.SGA.DataModel.Interfaces;
using System;
using System.Collections.Generic;

namespace PlayGen.SGA.DataModel
{
    public class GroupData : Contracts.Data, IModificationHistory
    {
        public int GroupId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
