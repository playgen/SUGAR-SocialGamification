using System;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
    public class ProjectFixture : IDisposable
    {
        public const string ConnectionString = "Server=localhost;Port=3306;Database=sugarunittests;Uid=root;Pwd=t0pSECr3t;Convert Zero Datetime=true;Allow Zero Datetime=true";
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
