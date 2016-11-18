using Microsoft.Extensions.DependencyInjection;

namespace PlayGen.SUGAR.WebAPI
{
    public partial class Startup
    {
        private void ConfigureRouting(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p
                // TODO: this should be specified in config at each deployment
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Authorization ")));
        }
    }
}
