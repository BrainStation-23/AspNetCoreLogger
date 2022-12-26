using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApp.Logger.Interceptors;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Sql.Configurations
{
    public static class DbContextDependency
    {
        public static void AddDbContextDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<SqlQueryInterceptor>();

            var connectionString = configuration.GetConnectionString("WebAppConnection");
            services.AddDbContext<WebAppContext>((provider, options) =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.LogTo(Console.WriteLine);
                options.UseSqlServer(connectionString);

                options.AddInterceptors(provider.GetRequiredService<SqlQueryInterceptor>());
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services
             .AddIdentity<User, Role>(options =>
             {
                 options.Password.RequiredLength = 6;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireDigit = true;
                 options.Password.RequireLowercase = true;
                 options.Password.RequireUppercase = false;
             })
             .AddEntityFrameworkStores<WebAppContext>()
             .AddDefaultTokenProviders();

        }
    }
}
