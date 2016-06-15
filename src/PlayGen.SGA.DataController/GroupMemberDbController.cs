using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.DataController
{
    public class GroupMemberDbController : DbController
    {
        public GroupMemberDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }
}
