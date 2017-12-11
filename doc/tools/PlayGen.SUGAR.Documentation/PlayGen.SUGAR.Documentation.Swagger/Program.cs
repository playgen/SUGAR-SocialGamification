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
                
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{EnvironmentName.Development}.json")
                    .Build();

                var builder = WebHost.CreateDefaultBuilder()
                    .UseEnvironment(EnvironmentName.Development)
                    .UseConfiguration(configuration)
                    .UseStartup<Startup>();
                    
                using (var server = new TestServer(builder))
                using (var client = server.CreateClient())
                {
                    var content = client.GetStringAsync(configuration.GetValue<string>("Swagger:Endpoint")).Result;
                    File.WriteAllText(outFilePath, content);
                }
            }
        }
    }
}
