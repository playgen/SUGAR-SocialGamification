using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class UserData : Contracts.Data, IModificationHistory
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
