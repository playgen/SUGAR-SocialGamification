using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;

namespace PlayGen.SGA.DataController.UnitTests
{
    public abstract class TestDbController
    {
        public const string DbName = "sgadatacontrollerunittests";

        protected readonly string _nameOrConnectionString;

        private static bool _deletedDatabase = false;

        public TestDbController()
        {
            _nameOrConnectionString = "Server=localhost;" +
                                      "Port=3306;" +
                                      $"Database={DbName};" +
                                      "Uid=root;" +
                                      "Pwd=;" +
                                      "Convert Zero Datetime=true;" +
                                      "Allow Zero Datetime=true";

            if (!_deletedDatabase)
            {
                using (var context = new SGAContext(_nameOrConnectionString))
                {
                    if (context.Database.Connection.Database == DbName)
                    {
                        context.Database.Delete();
                        _deletedDatabase = true;
                    }
                }
            }
        }
    }
}