using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;
using WebApp.Core.Sqls;
using WebApp.Sql;

namespace WebApp.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAppContext _dbContext;
        //public IAuditTrailRepository AuditTrailRepository { get; }

        public UnitOfWork(WebAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SqlRepository<T> Repository<T>() where T : MasterEntity, new()
        {
            return new SqlRepository<T>(_dbContext);
        }

        public int Complete()
        {
            int rowAffected = _dbContext.SaveChanges();

            return rowAffected;
        }

        public async Task<int> CompleteAsync()
        {
            int rowAffected = await _dbContext.SaveChangesAsync();

            return rowAffected;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public async Task<IDbContextTransaction> CreateTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel);
            return transaction;
        }

        public IDbContextTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = _dbContext.Database.BeginTransaction(isolationLevel);
            return transaction;
        }
    }
}
