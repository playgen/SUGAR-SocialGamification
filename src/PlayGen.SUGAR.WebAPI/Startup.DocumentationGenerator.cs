using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
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
				var projectRoot = app.ApplicationBasePath.Split(new[] { app.ApplicationName }, StringSplitOptions.None)[0];
				return Path.Combine(projectRoot, @"PlayGen.SUGAR.Contracts\bin\Debug\net35\PlayGen.SUGAR.Contracts.xml");
			}
		}
	}
}