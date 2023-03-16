//using Microsoft.Extensions.DependencyInjection;
//using System;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Providers.Files.Repos;

//namespace WebApp.Logger.Providers.Files
//{
//    public class FileProvider : ILog
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public FileProvider(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<FileSqlLogRepository>(_serviceProvider);
//        public IErrorLogRepository Error => ActivatorUtilities.CreateInstance<FileErrorLogRepository>(_serviceProvider);
//        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<FileAuditLogRepository>(_serviceProvider);
//        public IRequestLogRepository Request => ActivatorUtilities.CreateInstance<FileRequestLogRepository>(_serviceProvider);
//    }
//}
