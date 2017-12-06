using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace PlayGen.SUGAR.Server.WebAPI.Documentation
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any() || args.Any(arg => new[] { "-h", "--help", "help" }.Contains(arg)))
            {
                Console.WriteLine("Only pass through a single parameter that is the output path of the generated file.");
            }
            else
            {
                var outFilePath = args[0];

                var a = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var builder = WebHost.CreateDefaultBuilder()
                    .UseStartup<Startup>()
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile("appsettings.Development.json")
                        .Build());

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
