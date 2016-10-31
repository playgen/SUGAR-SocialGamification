using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using PlayGen.SUGAR.Client.IntegrationTests;
using PlayGen.SUGAR.WebAPI;

namespace PlayGen.SUGAR.Client.UnitTests
{
    [SetUpFixture]
    public class ClientTestBase
    {
        protected readonly TestSUGARClient TestSugarClient = new TestSUGARClient();
        private TestServer _server;

        [SetUp]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        [TearDown]
        public void TearDown()
        {
            _server.Dispose();
        }
    }
}
