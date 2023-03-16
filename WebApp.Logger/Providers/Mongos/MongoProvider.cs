//using Microsoft.Extensions.DependencyInjection;
//using System;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Providers.Mongos.Repos;

//namespace WebApp.Logger.Providers.Mongos
//{
//    public class MongoDbProvider : ILog
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public MongoDbProvider(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<MongoSqlLogRepository>(_serviceProvider);
//        public IErrorLogRepository Error => ActivatorUtilities.CreateInstance<MongoErrorLogRepository>(_serviceProvider);
//        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<MongoAuditLogRepository>(_serviceProvider);
//        public IRequestLogRepository Request => ActivatorUtilities.CreateInstance<MongoRequestLogRepository>(_serviceProvider);
//    }
//}
