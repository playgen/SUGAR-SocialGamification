using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
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
			});

			services.ConfigureSwaggerGen(options =>
			{
				options.IncludeXmlComments(GetXmlCommentsPath());
			});
		}

		private void ConfigureDocumentationGenerator(IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUi();
		}

		private string GetXmlCommentsPath()
		{
			var app = PlatformServices.Default.Application;
			var path = Path.Combine(app.ApplicationBasePath, app.ApplicationName + ".xml");
			return path;
		}
	}
}