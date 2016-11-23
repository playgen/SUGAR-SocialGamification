using System;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests
{
	[CollectionDefinition("Project Fixture Collection")]
	public class ProjectFixtureCollection : ICollectionFixture<ProjectFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}

	public class ProjectFixture : IDisposable
	{
		private readonly Data.EntityFramework.UnitTests.ProjectFixture _dbProjectFixture;
	 
		public ProjectFixture()
		{
			_dbProjectFixture = new Data.EntityFramework.UnitTests.ProjectFixture();
			TestData.PopulateData();
		}

		public void Dispose()
		{
			_dbProjectFixture.Dispose();
		}
	}
}
