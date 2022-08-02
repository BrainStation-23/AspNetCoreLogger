using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Common.Collections;
using WebApp.Common.Sqls;
using WebApp.Service;
using WebApp.Service.Contract.Models;
using WebApp.Core.Sqls;
using WebApp.Core.Collections;

namespace WebApp.Services
{
    public class BaseService<TEntity, TModel> : IBaseService<TEntity, TModel> where TEntity : MasterEntity, new() where TModel : MasterModel
    {
        private readonly SqlRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = unitOfWork.Repository<TEntity>();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Paging<TModel>> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var pageEntities = await _unitOfWork.Repository<TEntity>().GetPageAsync(pageIndex, pageSize);
            //var data = _mapper.Map<Paging<TDto>>(pageEntities);

            var data = pageEntities.ToPagingModel<TEntity, TModel>(_mapper);

            return data;
        }

        public virtual async Task<List<TModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            var data = _mapper.Map<List<TModel>>(entities);

            return data;

        }

        public async Task<TModel> FindAsync(long Id)
        {
            var entity = await _repository.FirstOrDefaultAsync(Id);

            var data = _mapper.Map<TModel>(entity);

            return data;
        }

        public virtual async Task<TModel> FirstOrDefaultAsync(long id)
        {
            var entity = await _repository.FirstOrDefaultAsync(id);

            var data = _mapper.Map<TModel>(entity);

            return data;
        }

        public virtual async Task<TModel> InsertAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var added = await _repository.InsertAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TModel>(added);

            return data;
        }

        public virtual async Task<List<TModel>> InsertRangeAsync(List<TModel> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);

            var addeds = await _repository.InsertRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TModel>>(addeds);

            return data;
        }

        public virtual async Task<TModel> UpdateAsync(long id, TModel model)
        {
            model.Id = id;

            var entity = _mapper.Map<TEntity>(model);

            var updated = await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TModel>(updated);
            return data;
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var updated = await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TModel>(updated);
            return data;
        }

        public virtual async Task<List<TModel>> UpdateRangeAsync(List<TModel> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);

            var updateds = await _repository.UpdateRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TModel>>(updateds);

            return data;
        }

        public virtual async Task<TModel> DeleteAsync(long id)
        {
            var deleted = await _repository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TModel>(deleted);
            return data;
        }

        public virtual async Task<TModel> DeleteAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var deleted = await _repository.DeleteAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TModel>(deleted);

            return data;
        }

        public virtual async Task<List<TModel>> DeleteRangeAsync(List<TModel> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);
            var deleteds = await _repository.DeleteRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TModel>>(deleteds);
            return data;
        }
    }
}
