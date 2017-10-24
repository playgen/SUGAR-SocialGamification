using System;
using Xunit;

// todo change these to test the core layer (as that makes use of the ef layer but imposes restrictions too).

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class DeleteDatabaseFixture
	{
		public DeleteDatabaseFixture()
		{
			using (var context = ControllerLocator.ContextFactory.Create())
			{
				context.Database.EnsureDeleted();
			}
		}
	}

	[CollectionDefinition(nameof(DeleteDatabaseFixture))]
	public class DeleteDatabaseFixtureCollection : ICollectionFixture<DeleteDatabaseFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
