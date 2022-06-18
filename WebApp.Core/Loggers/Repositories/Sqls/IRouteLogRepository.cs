using System.Threading.Tasks;
using WebApp.Core.Contexts;
using WebApp.Core.Models;

namespace WebApp.Core.Loggers.Repositories
{
    public interface IRouteLogRepository
    {
        Task AddAsync(RequestModel requestModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}