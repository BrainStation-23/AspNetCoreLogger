using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Core.Sqls;
using WebApp.Service;

namespace WebApp.Services
{
    public class BaseService<T> : IBaseService<T> where T : MasterEntity, new()
    {
        private readonly SqlRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Repository<T>();
            _unitOfWork = unitOfWork;
        }

        public async Task<Paging<T>> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var data = await _unitOfWork.Repository<T>().GetPageAsync(pageIndex, pageSize);

            return data;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<T> FindAsync(long Id)
        {
            return _repository.FirstOrDefaultAsync(Id);
        }

        public virtual async Task<T> FirstOrDefaultAsync(long id)
        {
            return await _repository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            await _repository.InsertAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity;
        }

        public virtual async Task<List<T>> InsertRangeAsync(List<T> entities)
        {
            await _repository.InsertRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            return entities;
        }

        public virtual async Task<T> UpdateAsync(long id, T entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity;
        }

        public virtual async Task<List<T>> UpdateRangeAsync(List<T> entities)
        {
            await _repository.UpdateRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            return entities;
        }

        public virtual async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public virtual async Task DeleteRangeAsync(List<T> entities)
        {
            await _repository.DeleteRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
        }
    }
}
