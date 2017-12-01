using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace PlayGen.SUGAR.Server.WebAPI.Documentation
{
	class Program
	{
		static void Main(string[] args)
		{
			if (!args.Any() || args.Any(arg => new[] {"-h", "--help", "help"}.Contains(arg)))
			{
				Console.WriteLine("Only pass through a single parameter that is the output path of the generated file.");
			}
			else
			{
				var outFilePath = args[0];

				var builder = new WebHostBuilder()
					.UseStartup<Startup>()
					.UseEnvironment("Development");
				
				using (var server = new TestServer(builder))
				using (var client = server.CreateClient())
				{
					var content = client.GetStringAsync("swagger/v1/swagger.json").Result;
					File.WriteAllText(outFilePath, content);
				}
			}
		}
	}
}
