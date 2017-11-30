using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PlayGen.SUGAR.Server.WebAPI;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class ClientTestsFixture : IDisposable
	{
		public readonly SUGARClient SUGARClient;
		protected readonly TestServer Server;

		public ClientTestsFixture()
		{
			var builder = new WebHostBuilder()
				.UseStartup<Startup>()
				.UseEnvironment("Tests");

			Server = new TestServer(builder);

			Program.SetUp(Server.Host);

			var client = Server.CreateClient();
			var testHttpHandler = new HttpClientHandler(client);

			SUGARClient = new SUGARClient(Server.BaseAddress.AbsoluteUri, testHttpHandler);
		}

		public void Dispose()
		{
			Server.Dispose();
		}
	}

	[CollectionDefinition(nameof(ClientTestsFixture))]
	// Note: This class must be in the same assembly as the tests in order for xUnit to detect it
	public class ClientTestsFixtureCollection : ICollectionFixture<ClientTestsFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
