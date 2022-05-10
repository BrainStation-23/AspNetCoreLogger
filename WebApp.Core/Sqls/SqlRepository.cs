using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Core.Collections;
using WebApp.Core.Exceptions;

namespace WebApp.Core.Sqls
{
    public class SqlRepository<T> : ISqlRepository<T> where T : MasterEntity, new()
    {
        readonly DbSet<T> _dbSet;
        public readonly DbContext _dbContext;

        public SqlRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }

        public virtual IQueryable<T> Query() => _dbSet;

        #region paging
        public virtual async Task<Paging<T>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _dbContext.Set<T>().OrderByDescending(e => e.Id).PagingAsync(pageIndex, pageSize);
        }

        public virtual async Task<Paging<T>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).OrderByDescending(e => e.Id).PagingAsync(pageIndex, pageSize);
        }

        public virtual async Task<Paging<T>> GetPageAsync(int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes)
        {
            return await orderBy(includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include)))
                .PagingAsync(pageIndex, pageSize);
        }

        public virtual async Task<Paging<T>> GetPageAsync(int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes)
        {

            return await orderBy(includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include), c => c.Where(predicate)))
                .PagingAsync(pageIndex, pageSize);
        }

        public virtual async Task<Paging<TResult>> GetPageAsync<TResult>(int pageIndex,
           int pageSize,
           Expression<Func<T, bool>> predicate,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
           Expression<Func<T, TResult>> selector,
           params Expression<Func<T, object>>[] includes)
        {
            return await orderBy(includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include), c => c.Where(predicate)))
                .PagingAsync(selector, pageIndex, pageSize);
        }

        #endregion

        #region dropdown
        public virtual async Task<Dropdown<T>> GetDropdownAsync(Expression<Func<T, bool>> predicate, int size)
        {
            return await _dbContext.Set<T>().Where(predicate).OrderByDescending(e => e.Id).DropdownAsync(size);
        }

        public virtual async Task<Dropdown<TResult>> GetDropdownAsync<TResult>(Expression<Func<T, bool>> predicate,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
           Expression<Func<T, TResult>> selector,
           int size)
        {
            var query = _dbContext.Set<T>().Where(predicate);
            if (orderBy is not null)
                query = orderBy(query);

            return await query.DropdownAsync(selector, size);
        }
        #endregion

        //public IQueryable<TViewModel> QueryTo<TViewModel>() where TViewModel : class
        //    => Query().ProjectTo<TViewModel>(_mapper.ConfigurationProvider);

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                return await query.AnyAsync(predicate);
            else
                return await query.AnyAsync();
        }

        #region RAW_SQL

        public async Task<List<T>> RawSqlListAsync<T>(FormattableString sql) where T : class
            => await _dbContext.Set<T>().FromSqlInterpolated(sql).ToListAsync();

        public async Task<T> RawSqlFirstOrDefaultAsync<T>(FormattableString sql) where T : class
            => (await _dbContext.Set<T>().FromSqlInterpolated(sql).ToListAsync()).FirstOrDefault();

        public async Task RawSqlAsync(FormattableString sql)
            => await _dbContext.Database.ExecuteSqlInterpolatedAsync(sql);

        public async Task<List<T>> RawSqlListAsync<T>(string sql, params object[] parameters) where T : class
            => await _dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();

        public async Task<T> RawSqlFirstOrDefaultAsync<T>(string sql, params object[] parameters) where T : class
            => (await _dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync()).FirstOrDefault();

        public async Task RawSqlAsync(string sql, params object[] parameters)
            => await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

        #endregion

        #region crud

        public virtual async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task InsertRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task UpdateRangeAsync(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task<T> DeleteAsync(object id)
        {
            var entity = await FirstOrDefaultAsync(id);

            if (entity is null)
                throw new NotFoundException("Entity");

            _dbSet.Remove(entity);

            return entity;
        }


        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        #endregion

        #region first or default
        public async Task<T> GetAsNoTrackingAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);

            _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> FirstOrDefaultAsync(object id) => await _dbSet.FindAsync(id);

        //public virtual async Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class
        //{
        //    Expression<Func<T, bool>> lambda = LambdaBuilder.BuildLambdaForFindByKey<T>(id);
        //    return await Query().Where(lambda).ProjectTo<TViewModel>(_mapper.ConfigurationProvider)
        //        .SingleOrDefaultAsync();
        //}

        //public virtual async Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null,
        //       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    IQueryable<T> query = _dbSet;

        //    if (predicate != null)
        //        query = query.Where(predicate);

        //    if (orderBy != null)
        //        return await orderBy(query).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        //    else
        //        return await query.ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        //}

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await orderBy(_dbContext.Set<T>()).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return await includes.Aggregate(_dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.AsNoTracking().FirstOrDefaultAsync(predicate)
            ).ConfigureAwait(false);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes)
        {
            return await includes.Aggregate(orderBy(_dbContext.Set<T>()).AsQueryable(),
                (current, include) => current.Include(include),
                c => c.FirstOrDefaultAsync(predicate)).ConfigureAwait(false);
        }

        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await orderBy(_dbContext.Set<T>()).LastOrDefaultAsync(predicate);
        }
        #endregion

        #region get        
        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        //public virtual async Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null) where TViewModel : class
        //{
        //    if (predicate is not null)
        //        return await Query().Where(predicate).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync();

        //    return await Query().ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        //}

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await orderBy(
                _dbContext.Set<T>().Where(predicate)
            ).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            return await includes.Aggregate(
                _dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.Where(predicate)
            ).ToListAsync()
            .ConfigureAwait(false);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            params Expression<Func<T, object>>[] includes)
        {
            return await orderBy(
                includes.Aggregate(
                _dbContext.Set<T>().AsQueryable(),
                (current, include) => current.Include(include),
                c => c.Where(predicate)
            )).ToListAsync()
            .ConfigureAwait(false);
        }
        #endregion

        public virtual void Attach(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public virtual void Detach(T entity)
        {
            if (_dbContext.Entry(entity).State != EntityState.Detached)
                _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public IQueryable<TViewModel> QueryTo<TViewModel>() where TViewModel : class
        {
            throw new NotImplementedException();
        }

        public Task<TViewModel> FirstOrDefault<TViewModel>(object id) where TViewModel : class
        {
            throw new NotImplementedException();
        }

        public Task<TViewModel> FirstOrDefaultToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<TViewModel>> GetAllToAsync<TViewModel>(Expression<Func<T, bool>> predicate = null) where TViewModel : class
        {
            throw new NotImplementedException();
        }
    }
}
