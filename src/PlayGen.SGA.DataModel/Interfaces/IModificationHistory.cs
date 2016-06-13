using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataModel.Interfaces
{
    public interface IModificationHistory
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }
}