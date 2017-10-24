using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	[Collection(nameof(ClearDatabaseFixture))]
	public abstract class EntityFrameworkTestBase
    {
    }
}
