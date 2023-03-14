using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Hostings
{
    public class RetentionPolicyService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LogOption _logOptions;

        public RetentionPolicyService(
            IOptions<LogOption> logOptions
            , IServiceProvider serviceProvider
            )
        {
            _logOptions = logOptions.Value;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = new Timer(RetentionAsync, null, TimeSpan.Zero, TimeSpan.FromDays(30));

            await Task.FromResult(Task.CompletedTask);
        }

        public async void RetentionAsync(object state = null)
        {
            var retentionDate = GetRetentionTime();

            if (retentionDate != null)
            {
                DateTime date = (DateTime)retentionDate;

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    if (_logOptions.LogType.MustContain(LogType.Error.ToString()) is true)
                        await scope.ServiceProvider.GetService<IExceptionLogRepository>().RetentionAsync(date);

                    if (_logOptions.LogType.MustContain(LogType.Request.ToString()) is true)
                        await scope.ServiceProvider.GetService<IRouteLogRepository>().RetentionAsync(date);

                    if (_logOptions.LogType.MustContain(LogType.Audit.ToString()) is true)
                        await scope.ServiceProvider.GetService<IAuditLogRepository>().RetentionAsync(date);

                    if (_logOptions.LogType.MustContain(LogType.Sql.ToString()) is true)
                        await scope.ServiceProvider.GetService<ISqlLogRepository>().RetentionAsync(date);
                }
            }
        }

        public DateTime? GetRetentionTime()
        {
            string retentionDays = null;

            if (_logOptions.ProviderType == ProviderType.MSSql.ToString())
                retentionDays = _logOptions.Provider.MSSql.Retention;

            else if (_logOptions.ProviderType == ProviderType.File.ToString())
                retentionDays = _logOptions.Provider.File.Retention;

            else if (_logOptions.ProviderType == ProviderType.CosmosDb.ToString())
                retentionDays = _logOptions.Provider.CosmosDb.Retention;

            else
                retentionDays = _logOptions.Provider.Mongo.Retention;

            if (retentionDays is null && _logOptions.Retention is not null)
                retentionDays = _logOptions.Retention.ToString();

            if (retentionDays is null)
                return null;
            else
                retentionDays = retentionDays.ToLower();

            char ch = retentionDays[retentionDays.Length - 1];
            int mul = 1;

            if (ch == 'm')
                mul = 30;
            else if (ch == 'y')
                mul = 365;

            retentionDays = retentionDays.Remove(retentionDays.Length - 1);
            var currentDate = DateTime.UtcNow;

            return currentDate.AddDays(-Int32.Parse(retentionDays) * mul).Date;
        }
    }
}
