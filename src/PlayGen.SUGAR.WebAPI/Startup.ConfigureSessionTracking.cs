using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Core.Sessions;

namespace PlayGen.SUGAR.WebAPI
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
