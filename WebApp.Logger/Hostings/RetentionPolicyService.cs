using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Hostings
{
    public class RetentionPolicyService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LogOption _logOptions;

        public RetentionPolicyService(IOptions<LogOption> logOptions
            , IServiceProvider serviceProvider)
        {
            _logOptions = logOptions.Value;
            _serviceProvider = serviceProvider;
        }

        private string GetRetentionConfig(LogOption logOption)
        {
            string retentionDays;
            if (logOption.ProviderType == ProviderType.MSSql.ToString())
                retentionDays = logOption.Provider.MSSql.Retention;
            else if (logOption.ProviderType == ProviderType.File.ToString())
                retentionDays = logOption.Provider.File.Retention;
            else if (logOption.ProviderType == ProviderType.CosmosDb.ToString())
                retentionDays = logOption.Provider.CosmosDb.Retention;
            else
                retentionDays = logOption.Provider.Mongo.Retention;

            if (retentionDays is null && logOption.Retention is not null)
                retentionDays = logOption.Retention.ToString();

            if (retentionDays is null)
                return null;
            else
                retentionDays = retentionDays.ToLower();

            return retentionDays;
        }

        private int ParseDays(string retentionString)
        {
            if (string.IsNullOrEmpty(retentionString)) return 0;
            if (retentionString.Length < 2) return 0;

            char postfix = retentionString[retentionString.Length - 1];
            int days = 1;

            if (postfix == 'm')
                days = 30;
            else if (postfix == 'y')
                days = 365;

            string retentionDayString = retentionString.Remove(retentionString.Length - 1);
            int retentionDays = Int32.Parse(retentionDayString);
            int totalDays = retentionDays * days;

            return totalDays;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = new Timer(RetentionAsync, null, TimeSpan.Zero, TimeSpan.FromDays(30));

            await Task.FromResult(Task.CompletedTask);
        }

        public async void RetentionAsync(object state = null)
        {
            string retentionDays = GetRetentionConfig(_logOptions);
            if (string.IsNullOrWhiteSpace(retentionDays)) return;

            int totalDays = ParseDays(retentionDays);
            var currentDate = DateTime.UtcNow;
            var date = currentDate.AddDays(-totalDays).Date;

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                if (_logOptions.LogType.MustContain(LogType.Error.ToString()) is true)
                    await scope.ServiceProvider.GetService<IErrorLogRepository>().RetentionAsync(date);

                if (_logOptions.LogType.MustContain(LogType.Request.ToString()) is true)
                    await scope.ServiceProvider.GetService<IRequestLogRepository>().RetentionAsync(date);

                if (_logOptions.LogType.MustContain(LogType.Audit.ToString()) is true)
                    await scope.ServiceProvider.GetService<IAuditLogRepository>().RetentionAsync(date);

                if (_logOptions.LogType.MustContain(LogType.Sql.ToString()) is true)
                    await scope.ServiceProvider.GetService<ISqlLogRepository>().RetentionAsync(date);
            }
        }
    }
}
