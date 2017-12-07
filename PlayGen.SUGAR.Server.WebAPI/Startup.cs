using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PlayGen.SUGAR.Server.Authentication;
using PlayGen.SUGAR.Server.Authentication.Filters;
using PlayGen.SUGAR.Server.WebAPI.Filters;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public partial class Startup
	{
		private const string TokenAudience = "User";

		private const string TokenIssuer = "SUGAR";
		private SymmetricSecurityKey key;
		private TokenAuthOptions tokenOptions;
		
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var timeoutCheckInterval = JsonConvert.DeserializeObject<TimeSpan>(Configuration["TimeoutCheckInterval"]);
			var validityTimeout = JsonConvert.DeserializeObject<TimeSpan>(Configuration["TokenValidityTimeout"]);
						
			services.AddApplicationInsightsTelemetry(Configuration);

			// Add framework services.
			services.AddMvc(options =>
			{
				options.Filters.Add(typeof(ModelValidationFilter));
				options.Filters.Add(typeof(ExceptionFilter));
				options.Filters.Add(typeof(WrapResponseFilter));
				options.Filters.Add(typeof(TokenReissueFilter));
				options.Filters.Add(typeof(SessionFilter));
				options.Filters.Add(typeof(APIVersionFilterFilter));
			})
			.AddJsonOptions(json =>
			{
				json.SerializerSettings.Converters.Add(new StringEnumConverter());
				json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			});

			ConfigureDbContextFactory(services);
			ConfigureDbControllers(services);
			ConfigureCoreControllers(services);
			ConfigureGameDataControllers(services);
			ConfigureRouting(services);
			ConfigureRESTAPIDocumentationGenerator(services);
			ConfigureAuthorization(services, validityTimeout);
			ConfigureAuthentication(services);
			ConfigureEvaluationEvents(services);
			ConfigureSessionTracking(services, validityTimeout, timeoutCheckInterval);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseCors("AllowAll");
			app.UseMvc();

			UseRESTAPIDocumentation(app, env);
		}
	}
}