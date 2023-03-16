using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Providers.CosmosDbs;

namespace WebApp.Logger.Providers.Mongos.Configurations
{
    public static class CosmosDependency
    {
        public static void AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ICosmosDbRepository<>), typeof(CosmosDbRepository<>));

            services.AddScoped<IErrorLogRepository, CosmosDbExceptionLogRepository>();
            services.AddScoped<IRequestLogRepository, CosmosDbRouteLogRepository>();
            services.AddScoped<IAuditLogRepository, CosmosDbAuditLogRepository>();
            services.AddScoped<ISqlLogRepository, CosmosDbSqlLogRepository>();
        }
    }
}
