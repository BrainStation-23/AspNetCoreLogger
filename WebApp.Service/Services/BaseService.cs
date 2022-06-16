using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Core.Sqls;
using WebApp.Service;

namespace WebApp.Services
{
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : MasterEntity, new() where TDto : class
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

        public async Task<Paging<TDto>> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var pageEntities = await _unitOfWork.Repository<TEntity>().GetPageAsync(pageIndex, pageSize);
            //var data = _mapper.Map<Paging<TDto>>(pageEntities);

            var data = pageEntities.ToPagingModel<TEntity, TDto>(_mapper);

            return data;
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var data = _mapper.Map<List<TDto>>(entities);

            return data;

        }

        public async Task<TDto> FindAsync(long Id)
        {
            var entity = await _repository.FirstOrDefaultAsync(Id);
            var data = _mapper.Map<TDto>(entity);

            return data;
        }

        public virtual async Task<TDto> FirstOrDefaultAsync(long id)
        {
            var entity = await _repository.FirstOrDefaultAsync(id);
            var data = _mapper.Map<TDto>(entity);

            return data;
        }

        public virtual async Task<TDto> InsertAsync(TDto model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var added = await _repository.InsertAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TDto>(added);

            return data;
        }

        public virtual async Task<List<TDto>> InsertRangeAsync(List<TDto> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);

            var addeds = await _repository.InsertRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TDto>>(addeds);

            return data;
        }

        public virtual async Task<TDto> UpdateAsync(long id, TDto model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var updated = await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TDto>(updated);
            return data;
        }

        public virtual async Task<TDto> UpdateAsync(TDto model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var updated = await _repository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TDto>(updated);
            return data;
        }

        public virtual async Task<List<TDto>> UpdateRangeAsync(List<TDto> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);

            var updateds = await _repository.UpdateRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TDto>>(updateds);

            return data;
        }

        public virtual async Task<TDto> DeleteAsync(long id)
        {
            var deleted = await _repository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TDto>(deleted);
            return data;
        }

        public virtual async Task<TDto> DeleteAsync(TDto model)
        {
            var entity = _mapper.Map<TEntity>(model);

            var deleted = await _repository.DeleteAsync(entity);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<TDto>(deleted);

            return data;
        }

        public virtual async Task<List<TDto>> DeleteRangeAsync(List<TDto> models)
        {
            var entities = _mapper.Map<List<TEntity>>(models);
            var deleteds = await _repository.DeleteRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            var data = _mapper.Map<List<TDto>>(deleteds);
            return data;
        }
    }
}
