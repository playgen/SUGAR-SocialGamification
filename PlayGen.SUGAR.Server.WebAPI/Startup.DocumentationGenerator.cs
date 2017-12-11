using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace PlayGen.SUGAR.Server.WebAPI
{
	public partial class Startup
	{
		private void ConfigureRESTAPIDocumentationGenerator(IServiceCollection services, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				services.AddSwaggerGen(options =>
				{
					var version = Configuration.GetValue<string>("Swagger:Version");
					var title = Configuration.GetValue<string>("Swagger:Title");
					var description = Configuration.GetValue<string>("Swagger:Description");
					options.SwaggerDoc(version, new Info
					{
						Version = version,
						Title = title,
						Description = description
					});

					options.DescribeAllEnumsAsStrings();
					options.IncludeXmlComments(APIXmlCommentsPath);
					options.IncludeXmlComments(ContractsXmlCommentsPath);
				});
			}
		}

		private void UseRESTAPIDocumentation(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();

				app.UseSwaggerUI(options =>
				{
					var endpoint = Configuration.GetValue<string>("Swagger:Endpoint");
					var description = Configuration.GetValue<string>("Swagger:Description");
					options.SwaggerEndpoint(endpoint, description);
				});
			}
		}

		private string APIXmlCommentsPath
		{
			get
			{
				var assemblyLocation = Assembly.Load(new AssemblyName("PlayGen.SUGAR.Server.WebAPI")).Location;
				return Path.ChangeExtension(assemblyLocation, ".xml");
			}
		}

		private string ContractsXmlCommentsPath
		{
			get
			{
				var assemblyLocation = Assembly.Load(new AssemblyName("PlayGen.SUGAR.Contracts")).Location;
				return Path.ChangeExtension(assemblyLocation, ".xml");
			}
		}
	}
}