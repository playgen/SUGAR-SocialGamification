using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
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
				return Path.Combine(app.ApplicationBasePath, @"PlayGen.SUGAR.Contracts.xml");
			}
		}

		private void ConfigureDocumentationGeneratorServices(IServiceCollection services)
		{
			services.AddSwaggerGen();

			services.ConfigureSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();

				options.IncludeXmlComments(APIXmlCommentsPath);
				options.IncludeXmlComments(ContractsXmlCommentsPath);
			});
		}

		private void ConfigureDocumentationGenerator(IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUi();
		}
	}
}