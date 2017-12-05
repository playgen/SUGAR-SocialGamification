using Xunit;

// todo change these to test the core layer (as that makes use of the ef layer but imposes restrictions too).

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class ClearDatabaseFixture
	{
		public ClearDatabaseFixture()
		{
			using (var context = ControllerLocator.ContextFactory.Create())
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
			}
		}
	}

	[CollectionDefinition(nameof(ClearDatabaseFixture))]
	public class ClearDatabaseFixtureCollection : ICollectionFixture<ClearDatabaseFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
