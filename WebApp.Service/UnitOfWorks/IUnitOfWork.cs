using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;
using WebApp.Common.Sqls;
using WebApp.Core.Sqls;

namespace WebApp.Service
{
    public interface IUnitOfWork
    {
        // Repositories
        SqlRepository<T> Repository<T>() where T : MasterEntity, new();
        //IAuditTrailRepository AuditTrailRepository { get; }

        int Complete();
        Task<int> CompleteAsync();

        void Disposes();
        Task DisposeAsync();

        Task<IDbContextTransaction> CreateTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
