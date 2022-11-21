using System;
using WebApp.Logger.Loggers.Providers.Sqls;

namespace WebApp.Logger.Loggers
{
    public class ProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILog Build(string providerType = null)
        {
            ILog log;
            providerType = providerType.ToLower();

            switch (providerType)
            {
                case "mssql":
                    log = new SqlProvider(_serviceProvider);
                    break;
                case "file":
                    log = new FileProvider(_serviceProvider);
                    break;
                //case ProviderType.MongoDb:
                //    log = new SqlProvider(_serviceProvider);
                //    break;
                //case ProviderType.CosmosDb:
                //    log = new SqlProvider(_serviceProvider);
                //    break;
                default:
                    log = new FileProvider(_serviceProvider);
                    break;
            }

            return log;
        }

        public ILog Build(ProviderType providerType)
        {
            ILog log;

            switch (providerType)
            {
                case ProviderType.MSSql:
                    log = new SqlProvider(_serviceProvider);
                    break;
                case ProviderType.File:
                    log = new FileProvider(_serviceProvider);
                    break;
                case ProviderType.MongoDb:
                    log = new MongoDbProvider(_serviceProvider);
                    break;
                case ProviderType.CosmosDb:
                    log = new CosmoDbProvider(_serviceProvider);
                    break;
                default:
                    log = new FileProvider(_serviceProvider);
                    break;
            }

            return log;
        }
    }
}
