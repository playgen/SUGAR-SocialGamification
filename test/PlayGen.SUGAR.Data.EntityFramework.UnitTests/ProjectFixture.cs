using System;
using Xunit;

// todo change these to test the core layer (as that makes use of the ef layer but imposes restrictions too).

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class ProjectFixture : IDisposable
	{
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
			using (var context = ControllerLocator.ContextFactory.Create())
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
