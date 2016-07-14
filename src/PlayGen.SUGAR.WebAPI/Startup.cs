using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.GameData;
using NLog.Extensions.Logging;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
		private static Logger _logger = LogManager.GetCurrentClassLogger();

		public Startup(IHostingEnvironment env)
		{
			#region Logging
			env.ConfigureNLog("NLog.config");
			_logger.Debug("ContentRootPath: {0}", env.ContentRootPath);
			_logger.Debug("WebRootPath: {0}", env.WebRootPath);
			#endregion

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
			services.AddScoped((_) => new ActorController(connectionString));
			services.AddScoped((_) => new GameDataController(connectionString));
			services.AddScoped((_) => new Data.EntityFramework.Controllers.AchievementController(connectionString));
			services.AddScoped((_) => new Data.EntityFramework.Controllers.SkillController(connectionString));
			services.AddScoped((_) => new Data.EntityFramework.Controllers.LeaderboardController(connectionString));
			services.AddScoped((_) => new GameData.ResourceController(connectionString));
			services.AddScoped((_) => new GroupRelationshipController(connectionString));
			services.AddScoped((_) => new UserRelationshipController(connectionString));

			// TODO set category types for GameDataControllers used by other controllers
			services.AddScoped((_) => new GameData.AchievementController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString), new ActorController(connectionString),
										new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString))));
			services.AddScoped((_) => new GameData.SkillController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString), new ActorController(connectionString),
										new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString))));
			services.AddScoped((_) => new RewardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), new UserRelationshipController(connectionString)));

			services.AddScoped((_) => new GameData.LeaderboardController(new GameDataController(connectionString), new GroupRelationshipController(connectionString), 
				new UserRelationshipController(connectionString), new ActorController(connectionString), new GroupController(connectionString),
				new UserController(connectionString)));

			services.AddScoped((_) => new PasswordEncryption());

			var apiKey = Configuration["APIKey"];
			services.AddScoped((_) => new JsonWebTokenUtility(apiKey));

			ConfigureRouting(services);
			// Add framework services.
			services.AddMvc(options =>
			{
				options.Filters.Add(new ModelValidationFilter());
				options.Filters.Add(typeof(AuthorizationHeaderFilter));
			});

			services.AddScoped<AuthorizationAttribute>();

			ConfigureDocumentationGeneratorServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddNLog();
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			ConfigureCors(app);
			app.UseMvc();

			ConfigureDocumentationGenerator(app);

		}

		private static void ConfigureRouting(IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy("AllowAll", p => p
				// TODO: this should be specified in config at each deployment
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.WithExposedHeaders(new [] { "Authorization "})));
		}

		private static void ConfigureCors(IApplicationBuilder application)
		{
			application.UseCors("AllowAll");
		}
	}
}
