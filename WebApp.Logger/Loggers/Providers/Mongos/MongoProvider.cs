using Microsoft.Extensions.DependencyInjection;
using System;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers.Providers.Sqls
{
    public class MongoDbProvider : ILog
    {
        private readonly IServiceProvider _serviceProvider;

        public MongoDbProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<MongoSqlLogRepository>(_serviceProvider);
        public IExceptionLogRepository Error => ActivatorUtilities.CreateInstance<MongoExceptionLogRepository>(_serviceProvider);
        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<MongoAuditLogRepository>(_serviceProvider);
        public IRouteLogRepository Request => ActivatorUtilities.CreateInstance<MongoRouteLogRepository>(_serviceProvider);
    }
}
