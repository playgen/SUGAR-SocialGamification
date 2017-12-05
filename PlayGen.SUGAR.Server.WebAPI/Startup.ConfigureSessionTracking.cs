using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Server.Core.Sessions;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureSessionTracking(IServiceCollection services, TimeSpan sessionTimeout, TimeSpan timeoutCheckInterval)
        {
            services.AddSingleton(serviceProvider => new SessionTracker(serviceProvider.GetService<ILogger<SessionTracker>>(), sessionTimeout, timeoutCheckInterval));
        }
    }
}
