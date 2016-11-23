using System;
using System.Collections.Generic;
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
	    private static bool _isInitialized;
	    private int _instanceCount;
        private static bool _isDisposed;

        public ProjectFixture()
        {
            _instanceCount++;

            if (!_isInitialized)
            {
                _dbProjectFixture = new Data.EntityFramework.UnitTests.ProjectFixture();
                _isInitialized = true;
            }
        }

		public virtual void Dispose()
		{
		    _instanceCount--;

            if (!_isDisposed && _instanceCount == 0)
		    {
		        _dbProjectFixture.Dispose();
		        _isDisposed = true;
		    }
		}
	}
}
