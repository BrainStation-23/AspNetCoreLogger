using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Core.Collections;

namespace WebApp.Core.Sqls
{
    public interface ISqlRepository<T> where T : class
    {
        IQueryable<T> Query();

        Task<Paging<T>> GetPageAsync(int pageIndex, int pageSize);
        Task<Paging<T>> GetPageAsync(int pageIndex, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, params Expression<Func<T, object>>[] includes);

        Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null) where TViewModel : class;
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, params Expression<Func<T, object>>[] includes);

        Task<List<T>> RawSqlListAsync(FormattableString sql);
        Task<List<T>> RawSqlListAsync(string sql, params object[] parameters);

        Task<T> GetAsNoTrackingAsync(object id);


        Task<T> FirstOrDefaultAsync(object id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes);
        Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class;

        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);

        Task RawSqlAsync(FormattableString sql);
        Task RawSqlAsync(string sql, params object[] parameters);
        Task<T> RawSqlFirstOrDefaultAsync(FormattableString sql);
        Task<T> RawSqlFirstOrDefaultAsync(string sql, params object[] parameters);

        Task InsertAsync(T entity);
        Task InsertRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(List<T> entities);

        Task<T> DeleteAsync(object id);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);

        void Attach(T entity);
        void Detach(T entity);
    }
}
