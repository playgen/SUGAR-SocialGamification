using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.ServerAuthentication;

namespace PlayGen.SUGAR.WebAPI
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();

		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Set EntityFramework's DBContext's connection string
			string connectioString = Configuration.GetConnectionString("DefaultConnection");
			using (var db = new SGAContext(connectioString))
			{
				db.Database.Initialize(true);
			}
			services.AddScoped((_) => new AccountController(connectioString));
			services.AddScoped((_) => new GameController(connectioString));
			services.AddScoped((_) => new GroupController(connectioString));
			services.AddScoped((_) => new UserController(connectioString));
			services.AddScoped((_) => new GroupDataController(connectioString));
			services.AddScoped((_) => new UserDataController(connectioString));
			services.AddScoped((_) => new GroupAchievementController(connectioString));
			services.AddScoped((_) => new UserAchievementController(connectioString));
			services.AddScoped((_) => new GroupRelationshipController(connectioString));
			services.AddScoped((_) => new UserRelationshipController(connectioString));

			services.AddScoped((_) => new PasswordEncryption());

			var apiKey = Configuration["APIKey"];
			services.AddScoped((_) => new JsonWebTokenUtility(apiKey));

			ConfigureRouting(services);
			
			// Add framework services.
			services.AddMvc();
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			ConfigureCors(app);
			app.UseMvc();
		}

		private static void ConfigureRouting(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
		}

		private static void ConfigureCors(IApplicationBuilder application)
		{
			application.UseCors("AllowAll");
		}
	}
}
