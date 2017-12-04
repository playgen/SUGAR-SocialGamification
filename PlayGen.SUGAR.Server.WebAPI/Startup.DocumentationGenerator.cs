using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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