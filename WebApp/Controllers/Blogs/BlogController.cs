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
    public class BlogController : GenericBaseController<BlogEntity>
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) : base(blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _blogService.GetSearchAsync(pageIndex, pageSize, searchText);

            return new OkResponse(res);
        }
    }
}
