﻿using System;
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
	}
}
