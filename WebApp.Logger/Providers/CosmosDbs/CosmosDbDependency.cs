using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Logger.Contracts;
using WebApp.Logger.Providers.CosmosDbs;
using WebApp.Logger.Providers.CosmosDbs.Repos;

namespace WebApp.Logger.Providers.Mongos.Configurations
{
    public static class CosmosDependency
    {
        public static void AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ICosmosDbRepository<>), typeof(CosmosDbRepository<>));

            services.AddScoped<IErrorLogRepository, CosmosDbErrorLogRepository>();
            services.AddScoped<IRequestLogRepository, CosmosDbRequestLogRepository>();
            services.AddScoped<IAuditLogRepository, CosmosDbAuditLogRepository>();
            services.AddScoped<ISqlLogRepository, CosmosDbSqlLogRepository>();
        }
    }
}
