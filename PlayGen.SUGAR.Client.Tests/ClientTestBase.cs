using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.EntityFramework.Tests;
using PlayGen.SUGAR.Server.WebAPI;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	[Collection(nameof(ClearDatabaseFixture))]
	public abstract class ClientTestBase : IDisposable
	{
		protected readonly SUGARClient SUGARClient;
		protected readonly TestServer Server;

		protected ClientTestBase(string environment = "Tests")
		{
			var builder = new WebHostBuilder()
				.UseStartup<Startup>()
				.UseEnvironment(environment);
			
			Server = new TestServer(builder);
			var client = Server.CreateClient();
			var testHttpHandler = new HttpClientHandler(client);
			
			SUGARClient = new SUGARClient(Server.BaseAddress.AbsoluteUri, testHttpHandler);

			LoginAdmin();
		}

		public void Dispose()
		{
			try
			{
				SUGARClient.Session.Logout();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Server.Dispose();
		}

		public AccountResponse LoginAdmin()
		{
			AccountResponse response;

			var accountRequest = new AccountRequest
			{
				Name = "admin",
				Password = "admin",
				SourceToken = "SUGAR",
			};

			try
			{
				response = SUGARClient.Session.Login(accountRequest);
			}
			catch(Exception ex)
			{
				response = SUGARClient.Session.CreateAndLogin(accountRequest);
			}

			return response;
		}
	}
}
