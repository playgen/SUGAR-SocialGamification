using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	[Collection(nameof(CoreTestFixtureCollection))]
    public class CoreTestFixtureBase
    {
        public CoreTestFixture Fixture { get; }

        public CoreTestFixtureBase(CoreTestFixture fixture)
        {
            Fixture = fixture;
        }
    }
}
