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

        IQueryable<TViewModel> QueryTo<TViewModel>() where TViewModel : class;
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);


        Task<List<T>> RawSqlListAsync<T>(FormattableString sql) where T : class;

        Task<T> RawSqlFirstOrDefaultAsync<T>(FormattableString sql) where T : class;

        Task RawSqlAsync(FormattableString sql);

        Task<List<T>> RawSqlListAsync<T>(string sql, params object[] parameters) where T : class;

        Task<T> RawSqlFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class;

        Task RawSqlAsync(string sql, params object[] parameters);


        Task InsertAsync(T entity);

        Task InsertRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(List<T> entities);

        Task<T> DeleteAsync(object id);

        Task DeleteRangeAsync(List<T> entities);

        Task DeleteAsync(T entity);


        Task<T> FirstOrDefaultAsync(object id);
        Task<T> GetAsNoTrackingAsync(object id);

        Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class;

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate
        );

        Task<T> LastOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy
        );

        Task<T> FirstOrDefaultAsync(
           Expression<Func<T, bool>> predicate,
           params Expression<Func<T, object>>[] includes
       );

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy
        );

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes
        );

        //TODO: multiple include acceptable
        Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
         );

        Task<List<T>> GetAllAsync();

        Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null) where TViewModel : class;

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] include
        );

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy
        );

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes
        );



        void Attach(T entity);
        void Detach(T entity);
    }
}
