using System.Threading.Tasks;

namespace WebApp.Logger.Contracts
{
    public interface IDashboardRepository
    {
        Task<dynamic> GetTopRequestsAsync();

        Task<dynamic> GetTopExceptionAsync();

        Task<dynamic> GetSummaryAsync();

        Task<dynamic> GetSlowestRequestAsync();

        Task<dynamic> GetFastestRequestAsync();
    }
}
