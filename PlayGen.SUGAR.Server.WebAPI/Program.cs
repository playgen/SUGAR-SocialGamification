using System.IO;
using Microsoft.AspNetCore.Hosting;

using PlayGen.SUGAR.Server.Core.Authorization;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			var factory = (SUGARContextFactory)host.Services.GetService(typeof(SUGARContextFactory));
			using (var context = factory.Create())
			{
				var newlyCreated = context.Database.EnsureCreated();
				if (newlyCreated)
				{
					context.Seed();
				}
				((ClaimController)host.Services.GetService(typeof(ClaimController))).GetAuthorizationClaims();
				if (((IHostingEnvironment)host.Services.GetService(typeof(IHostingEnvironment))).EnvironmentName == "Tests")
				{
					context.SeedTesting();
				}
			}

			host.Run();
		}
	}
}
