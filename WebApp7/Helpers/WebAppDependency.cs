using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Service.Configurations;

namespace WebApp7.Helpers
{
    public static class WebAppDependency
    {
        public static void AddWebAppDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(WebAppMapperProfile));

            services.AddServiceDependency(configuration);
        }
    }
}
