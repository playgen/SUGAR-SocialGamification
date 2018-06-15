using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PlayGen.SUGAR.Server.WebAPI;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	[Collection(nameof(ClientTestBase))]
	public abstract class ClientTestBase
	{
		protected readonly ClientTestsFixture Fixture;

		protected ClientTestBase(ClientTestsFixture fixture)
		{
			Fixture = fixture;

			if (!Fixture.SetupClassContexts.ContainsKey(GetType().Name))
			{
				// ReSharper disable once VirtualMemberCallInConstructor
				Fixture.SetupClassContexts.Add(GetType().Name, SetupClass(Fixture));
			}
		}

		/// <summary>
        /// Use this to do the setup for a test. Put any data on a context object and return that.
        /// Do not set any values on the calling class.
        /// Use this method in complete isolation.
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
		protected virtual object SetupClass(ClientTestsFixture fixture)
		{
			return null;
		}

		protected T GetSetupClassContext<T>()
		{
			return (T)Fixture.SetupClassContexts[GetType().Name];
		}
	}

	public class ClientTestsFixture : IDisposable
	{
		public readonly SUGARClient SUGARClient;
		public readonly Dictionary<string, object> SetupClassContexts = new Dictionary<string, object>();

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

	[CollectionDefinition(nameof(ClientTestBase))]
	// Note: This class must be in the same assembly as the tests in order for xUnit to detect it
	public class ClientTestsFixtureCollection : ICollectionFixture<ClientTestsFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
