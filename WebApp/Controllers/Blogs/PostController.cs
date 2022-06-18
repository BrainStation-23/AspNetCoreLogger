using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Entity.Entities.Blogs;
using WebApp.Helpers.Base;
using WebApp.Service;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : GenericBaseController<PostEntity, PostModel>
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService, IMapper mapper) : base(postService, mapper)
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
