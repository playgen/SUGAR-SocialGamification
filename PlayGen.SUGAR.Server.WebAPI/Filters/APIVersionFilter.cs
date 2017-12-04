using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.WebAPI.Exceptions;

namespace PlayGen.SUGAR.Server.WebAPI.Filters
{
    public class APIVersionFilterFilter : IActionFilter
    {
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		public void OnActionExecuting(ActionExecutingContext context)
        {
			if (context.HttpContext.Request.Headers.TryGetValue("APIVersion", out var requestApiVersion))
			{
				if (!APIVersion.IsCompatible(requestApiVersion))
				{
					throw new IncompatibleAPIVersionException($"Server and Client API Major versions do not match." +
															$" This is likley to cause unpredictable behaviour." +
															$" \nServer API Version: {requestApiVersion}" +
															$" \nClient API Version: {requestApiVersion}");
				}
			}
			else
			{
				Logger.Warn("Incoming request with no APIVersion specified: {0}", context.ActionDescriptor.AttributeRouteInfo.Template);
			}
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
