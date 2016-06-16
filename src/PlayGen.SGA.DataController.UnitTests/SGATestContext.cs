using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class SGATestContext : SGAContext
    {
        public SGATestContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }
}
