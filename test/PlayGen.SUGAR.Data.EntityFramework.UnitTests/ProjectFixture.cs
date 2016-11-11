using System;
using Xunit;

// todo change these to test the core layer (as that makes use of the ef layer but imposes restrictions too).

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
    public class ProjectFixture : IDisposable
    {
        // Pooling needs to be set to false due to a MySQL bug that reports "Nested transactions are not supported"
        // See: 
        // http://stackoverflow.com/questions/26320679/asp-net-web-forms-and-mysql-entity-framework-nested-transactions-are-not-suppo
        // http://bugs.mysql.com/bug.php?id=71502
        public const string ConnectionString = "Server=localhost;Port=3306;Database=sugarunittests;Uid=root;Pwd=t0pSECr3t;Convert Zero Datetime=true;Allow Zero Datetime=true;Pooling=false";
        public static readonly SUGARContextFactory ContextFactory = new SUGARContextFactory(ConnectionString);

        public ProjectFixture()
        {
            DeleteDatabase();

            // ... put any initialization code here
        }

        public void Dispose()
        {
            // ... put any cleanup code here
        }

        private void DeleteDatabase()
        {
            using (var context = ContextFactory.Create())
            {
                context.Database.EnsureDeleted();
            }
        }
    }

    [CollectionDefinition("Project Fixture Collection")]
    public class ProjectFixtureCollection : ICollectionFixture<ProjectFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
