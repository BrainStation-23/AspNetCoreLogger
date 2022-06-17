using System.Threading.Tasks;
using WebApp.Core.Contexts;
using WebApp.Core.Models;

namespace WebApp.Core.Loggers.Repositories
{
    public interface IExceptionLogRepository
    {
        Task AddAsync(ErrorModel errorModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}