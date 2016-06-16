using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;

namespace PlayGen.SGA.DataController.UnitTests
{
    public abstract class TestDbController
    {
        protected readonly string _nameOrConnectionString;
        public static bool hasTestRun = false;
        public const string DbName = "sgadatacontrollerunittests";

        public TestDbController()
        {
            _nameOrConnectionString = "Server=localhost;" +
                                      "Port=3306;" +
                                      "Database=" + DbName + ";" +
                                      "Uid=root;" +
                                      "Pwd=;" +
                                      "Convert Zero Datetime=true;" +
                                      "Allow Zero Datetime=true";

            using (var context = new SGAContext(_nameOrConnectionString))
            {
                if (context.Database.Connection.Database == DbName && !hasTestRun)
                {
                    context.Database.Delete();
                    hasTestRun = true;
                }
            }
        }
    }
}