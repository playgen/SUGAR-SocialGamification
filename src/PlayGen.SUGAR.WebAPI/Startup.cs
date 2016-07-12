using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.GameData;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;
using PlayGen.SUGAR.Data.ContextScope;

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

			services.AddSingleton<IContextScopeFactory>(new ContextScopeFactory(connectionString));
			services.AddSingleton<IAmbientContextLocator>(new AmbientContextLocator());
			
			// TODO migrate all of these to use the ContextScopeFactory so their constructors don't need to take the connection string.
			services.AddScoped((_) => new AccountController(connectionString));
			services.AddScoped((_) => new GameController(connectionString));
			services.AddScoped((_) => new GroupController(connectionString));
			services.AddScoped((_) => new UserController(connectionString));
			services.AddScoped((_) => new ActorController(connectionString));
			services.AddScoped<IGameDataController>((_) => new GameDataController(connectionString));
			services.AddScoped((new AmbientContextLocator()) => Data.EntityFramework.Controllers.AchievementController());
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
