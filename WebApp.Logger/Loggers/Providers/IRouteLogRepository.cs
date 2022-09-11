using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IRouteLogRepository
    {
        Task AddAsync(RequestModel requestModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}