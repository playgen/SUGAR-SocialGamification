using Microsoft.Extensions.DependencyInjection;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;

namespace PlayGen.SUGAR.Server.WebAPI
{
    public partial class Startup
    {
        private void ConfigureEvaluationEvents(IServiceCollection services)
        {
            services.AddScoped<ProgressEvaluator>();
            services.AddScoped<CriteriaEvaluator>();

            services.AddSingleton<EvaluationTracker>();
        }
    }
}
