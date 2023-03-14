using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebApp.Logger.Loggers
{
    public interface IDbContextExtension
    {
        void Configuring(DbContextOptionsBuilder optionsBuilder);
        Task<bool> AuditTrailLogAsync(DbContext context);
    }
}
