using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;

namespace WebApp.Services
{
    public interface IBaseService<TEntity, TDto> where TEntity : class where TDto: class
    {
        Task<Paging<TDto>> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize);
        Task<List<TDto>> GetAllAsync();
        Task<TDto> FindAsync(long Id);
        Task<TDto> FirstOrDefaultAsync(long id);
        Task<TDto> InsertAsync(TDto entity);
        Task<List<TDto>> InsertRangeAsync(List<TDto> entities);
        Task<TDto> UpdateAsync(long id, TDto entity);
        Task<TDto> UpdateAsync(TDto entity);
        Task<List<TDto>> UpdateRangeAsync(List<TDto> entities);
        Task<TDto> DeleteAsync(long id);
        Task<TDto> DeleteAsync(TDto entity);
        Task<List<TDto>> DeleteRangeAsync(List<TDto> entities);
    }
}
