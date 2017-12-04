using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureEvaluationEvents(IServiceCollection services)
        {
            services.AddSingleton<ProgressEvaluator>();
            services.AddSingleton<CriteriaEvaluator>();
            services.AddSingleton<EvaluationTracker>();
        }
    }
}
