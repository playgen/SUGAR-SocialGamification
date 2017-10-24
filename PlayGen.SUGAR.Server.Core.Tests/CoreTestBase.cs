using System;
using System.Collections.Generic;
using System.Text;
using PlayGen.SUGAR.Server.EntityFramework.Tests;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	[Collection(nameof(DeleteDatabaseFixture))]
	public abstract class CoreTestBase
    {
    }
}
