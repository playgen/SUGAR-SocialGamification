using System;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Core.Sessions;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureSessionTracking(IServiceCollection services, TimeSpan sessionTimeout, TimeSpan timeoutCheckInterval)
        {
            var sessionTracker = new SessionTracker(sessionTimeout, timeoutCheckInterval);
            services.AddSingleton(sessionTracker);
        }
    }
}
