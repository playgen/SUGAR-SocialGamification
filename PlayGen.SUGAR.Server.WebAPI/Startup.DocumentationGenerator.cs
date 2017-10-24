using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public partial class Startup
	{
		private void ConfigureDocumentationGeneratorServices(IServiceCollection services, IHostingEnvironment env)
		{
			if (!env.IsEnvironment("Tests"))
			{
				services.AddSwaggerGen();

				services.ConfigureSwaggerGen(options =>
				{
					options.DescribeAllEnumsAsStrings();

					options.IncludeXmlComments(APIXmlCommentsPath);
					options.IncludeXmlComments(ContractsXmlCommentsPath);
				});
			}
		}

		private void ConfigureDocumentationGenerator(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (!env.IsEnvironment("Tests"))
			{ 
				app.UseSwagger();
				app.UseSwaggerUi();
			}
		}

		private string APIXmlCommentsPath
		{
			get
			{
				var app = PlatformServices.Default.Application;
				return Path.Combine(app.ApplicationBasePath, app.ApplicationName + ".xml");
			}
		}

		private string ContractsXmlCommentsPath
		{
			get
			{
				var app = PlatformServices.Default.Application;
				return Path.Combine(app.ApplicationBasePath, "PlayGen.SUGAR.Contracts.xml");
			}
		}
	}
}