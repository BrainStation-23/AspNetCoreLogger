using System.Threading.Tasks;
using WebApp.Core.Models;

namespace WebApp.Core.Loggers.Repositories
{
    public interface IRouteLogRepository
    {
        Task AddAsync(RequestModel requestModel);
    }
}