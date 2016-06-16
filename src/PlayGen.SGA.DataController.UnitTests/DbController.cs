using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataController.UnitTests
{
    public abstract class DbController
    {
        protected readonly string _nameOrConnectionString = "Server=localhost;Port=3306;Database=SGA;Uid=root;Pwd=;Convert Zero Datetime=true;Allow Zero Datetime=true";
    }
}