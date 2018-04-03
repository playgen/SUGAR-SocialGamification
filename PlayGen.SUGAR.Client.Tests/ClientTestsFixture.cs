using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
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
			var builder = WebHost.CreateDefaultBuilder()
				.UseStartup<Startup>()
				.UseEnvironment("Tests");

			Server = new TestServer(builder);

			Program.Setup(Server.Host);

			SUGARClient = CreateSugarClient();
		}

		public SUGARClient CreateSugarClient(Dictionary<string, string> persistentHeaders = null, Dictionary<string, string> sessionHeaders = null)
		{
			var client = Server.CreateClient();
			var testHttpHandler = new HttpClientHandler(client);
			var sugarClient = new SUGARClient(Server.BaseAddress.AbsoluteUri, testHttpHandler, true, persistentHeaders, sessionHeaders);
			return sugarClient;
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
