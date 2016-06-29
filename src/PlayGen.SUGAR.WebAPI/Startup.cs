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
using PlayGen.SUGAR.WebAPI.Controllers.Filters;

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
			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			using (var db = new SGAContext(connectionString))
			{
				db.Database.Initialize(true);
			}
			services.AddScoped((_) => new AccountController(connectionString));
			services.AddScoped((_) => new GameController(connectionString));
			services.AddScoped((_) => new GroupController(connectionString));
			services.AddScoped((_) => new UserController(connectionString));
			services.AddScoped((_) => new GameDataController(connectionString));
			services.AddScoped((_) => new AchievementController(connectionString));
			services.AddScoped((_) => new GroupRelationshipController(connectionString));
			services.AddScoped((_) => new UserRelationshipController(connectionString));

			services.AddScoped((_) => new PasswordEncryption());

			var apiKey = Configuration["APIKey"];
			services.AddScoped((_) => new JsonWebTokenUtility(apiKey));

			ConfigureRouting(services);
			
			// Add framework services.
			services.AddMvc(options => options.Filters.Add(new ModelValidationFilter()));
			services.AddScoped<ArgumentsNotNullAttribute>();
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
