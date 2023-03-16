//using Microsoft.Extensions.DependencyInjection;
//using System;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Providers.CosmosDbs.Repos;

//namespace WebApp.Logger.Providers.CosmosDbs
//{
//    public class CosmoDbProvider : ILog
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public CosmoDbProvider(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<CosmosDbSqlLogRepository>(_serviceProvider);
//        public IErrorLogRepository Error => ActivatorUtilities.CreateInstance<CosmosDbErrorLogRepository>(_serviceProvider);
//        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<CosmosDbAuditLogRepository>(_serviceProvider);
//        public IRequestLogRepository Request => ActivatorUtilities.CreateInstance<CosmosDbRequestLogRepository>(_serviceProvider);
//    }
//}
