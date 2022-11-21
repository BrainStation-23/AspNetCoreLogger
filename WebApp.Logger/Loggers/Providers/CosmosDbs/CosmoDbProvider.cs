using Microsoft.Extensions.DependencyInjection;
using System;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers.Providers.Sqls
{
    public class CosmoDbProvider : ILog
    {
        private readonly IServiceProvider _serviceProvider;

        public CosmoDbProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<CosmosDbSqlLogRepository>(_serviceProvider);
        public IExceptionLogRepository Error => ActivatorUtilities.CreateInstance<CosmosDbExceptionLogRepository>(_serviceProvider);
        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<CosmosDbAuditLogRepository>(_serviceProvider);
        public IRouteLogRepository Request => ActivatorUtilities.CreateInstance<CosmosDbRouteLogRepository>(_serviceProvider);
    }
}
