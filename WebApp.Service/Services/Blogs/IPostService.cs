using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Service.Models.Blogs;
using WebApp.Services;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models;

namespace WebApp.Service
{
    public interface IPostService : IBaseService<PostEntity, PostDto>
    {
        Task<Paging<PostModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null);
    }
}
