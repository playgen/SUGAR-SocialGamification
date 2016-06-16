using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataController.UnitTests
{
    public abstract class DbController
    {
        protected readonly string _nameOrConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=DataControllerUnitTests";

    }
}