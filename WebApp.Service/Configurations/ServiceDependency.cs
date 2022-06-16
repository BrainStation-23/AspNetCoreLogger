using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core;
using WebApp.Core.Acls;
using WebApp.Services;
using WebApp.Sql.Configurations;

namespace WebApp.Service.Configurations
{
    public static class ServiceDependency
    {
        public static void AddServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositoryDependency();
            services.AddAutoMapper(typeof(ServiceMapperProfile));

            services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
            
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ISignInHelper, SignInHelper>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPostService, PostService>();
        }
    }
}
