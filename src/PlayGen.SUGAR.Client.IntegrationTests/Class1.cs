using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {
		[Fact]
	    public void Test()
	    {
		    Assert.False(false);
	    }

		[Fact]
		public void Test2()
		{
			Assert.False(true);
		}

		[Fact]
		public void Test3()
		{
			Assert.True(true);
		}
	}
}
