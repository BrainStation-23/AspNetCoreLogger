using System.Threading.Tasks;
using WebApp.Core.Contexts;

namespace WebApp.Core.Loggers.Repositories
{
    public interface IAuditLogRepository
    {
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}