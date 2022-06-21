using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Services;

namespace WebApp.Service
{
    public interface IPostService : IBaseService<PostEntity, PostModel>
    {
        Task<Paging<PostModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null);
    }
}
