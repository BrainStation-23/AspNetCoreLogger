using System.Threading.Tasks;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface ISqlLogRepository
    {
        Task AddAsync(SqlModel sqlModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}