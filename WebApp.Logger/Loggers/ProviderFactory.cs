//using System;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Providers.CosmosDbs.Repos;
//using WebApp.Logger.Providers.Files;
//using WebApp.Logger.Providers.Mongos.Repos;
//using WebApp.Logger.Providers.Sqls;

//namespace WebApp.Logger.Loggers
//{
//    public class ProviderFactory
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public ProviderFactory(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public ILog Build(string providerType = null)
//        {
//            providerType = providerType.ToLower();
//            ILog log = providerType switch
//            {
//                "mssql" => new SqlProvider(_serviceProvider),
//                "file" => new FileProvider(_serviceProvider),
//                "mongo" => new MongoDbProvider(_serviceProvider),
//                "cosmosdb" => new CosmoDbProvider(_serviceProvider),
//                _ => new FileProvider(_serviceProvider),
//            };
//            return log;
//        }

//        public ILog Build(ProviderType providerType)
//        {
//            ILog log = providerType switch
//            {
//                ProviderType.MSSql => new SqlProvider(_serviceProvider),
//                ProviderType.File => new FileProvider(_serviceProvider),
//                ProviderType.MongoDb => new MongoDbProvider(_serviceProvider),
//                ProviderType.CosmosDb => new CosmoDbProvider(_serviceProvider),
//                _ => new FileProvider(_serviceProvider),
//            };
//            return log;
//        }
//    }
//}
