using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Interceptors
{
    public class SqlSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor Context;
        private readonly ISqlLogRepository SqlLogRepository;
        private readonly LogOption _logOption;

        public SqlSaveChangesInterceptor(IHttpContextAccessor context,
            ISqlLogRepository sqlLogRepository,
            IOptions<LogOption> logOption)
        {
            Context = context;
            SqlLogRepository = sqlLogRepository;
            _logOption = logOption.Value;
        }

        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {

            return result;
        }

        public InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            return result;
        }
    }
}