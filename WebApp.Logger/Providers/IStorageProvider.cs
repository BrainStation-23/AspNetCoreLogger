using System.Threading.Tasks;

namespace WebApp.Logger.Providers
{
    public interface IStorageProvider
    {
        Task AddAsync();
        Task GetAsync();
        Task GetByTraceIdAsync();
    }
}
