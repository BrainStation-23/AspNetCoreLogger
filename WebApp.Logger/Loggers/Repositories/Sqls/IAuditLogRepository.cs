using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IAuditLogRepository
    {
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}