﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;
using PlayGen.SUGAR.Core.Utilities;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.ServerAuthentication.Filters;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
		private const string TokenAudience = "User";
		private const string TokenIssuer = "SUGAR";
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private SymmetricSecurityKey key;
		private TokenAuthOptions tokenOptions;


		public Startup(IHostingEnvironment env)
		{
			//AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			#region Logging

			ConfigureNLog(env);

			Logger.Debug("ContentRootPath: {0}", env.ContentRootPath);
			Logger.Debug("WebRootPath: {0}", env.WebRootPath);

			#endregion

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);


			if (env.IsEnvironment("Development"))
				builder.AddApplicationInsightsSettings(true);

			builder.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		//private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		//{
		//	Logger.Error($"AppDomain UnhandledException: {e.ExceptionObject}");
		//}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var timeoutCheckInterval = JsonConvert.DeserializeObject<TimeSpan>(Configuration["TimeoutCheckInterval"]);
			var validityTimeout = JsonConvert.DeserializeObject<TimeSpan>(Configuration["TokenValidityTimeout"]);

			services.AddScoped(_ => new PasswordEncryption());
			services.AddApplicationInsightsTelemetry(Configuration);

			// Add framework services.
			services.AddMvc(options =>
				{
					options.Filters.Add(new ModelValidationFilter());
					options.Filters.Add(new ExceptionFilter());
					options.Filters.Add(typeof(WrapResponseFilter));
					options.Filters.Add(typeof(TokenReissueFilter));
					options.Filters.Add(typeof(SessionFilter));
				})
				.AddJsonOptions(json =>
				{
					json.SerializerSettings.Converters.Add(new StringEnumConverter());
					json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					json.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
				});

			ConfigureDbContextFactory(services);
			ConfigureDbControllers(services);
			ConfigureCoreControllers(services);
			ConfigureGameDataControllers(services);
			ConfigureRouting(services);
			ConfigureDocumentationGeneratorServices(services);
			ConfigureAuthorization(services, validityTimeout);
			ConfigureEvaluationEvents(services);
			ConfigureSessionTracking(services, validityTimeout, timeoutCheckInterval);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			ConfigureLogging(loggerFactory);
			ConfigureAuthentication(app);

			app.UseCors("AllowAll");
			app.UseApplicationInsightsRequestTelemetry();
			app.UseApplicationInsightsExceptionTelemetry();
			app.UseMvc();

			ConfigureDocumentationGenerator(app);
		}
	}
}