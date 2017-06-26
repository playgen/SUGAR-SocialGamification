using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace PlayGen.SUGAR.WebAPI
{
	public partial class Startup
	{
		private void ConfigureNLog(IHostingEnvironment env)
		{
			env.ConfigureNLog("NLog.config");
		}

		private void ConfigureLogging(ILoggerFactory loggerFactory)
		{
			loggerFactory.AddNLog();
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
		}
	}
}