using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Helpers.Base;
using WebApp.Service;
using WebApp.Sql.Entities.Blogs;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : GenericBaseController<PostEntity>
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService) : base(postService)
        {
            _postService = postService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _postService.GetSearchAsync(pageIndex, pageSize, searchText);

            return new OkResponse(res);
        }
    }
}
