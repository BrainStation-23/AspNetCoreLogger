using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Service;
using WebApp.ViewModels.Enums;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ExceptionController
    {
        private readonly IBlogService _blogService;

        public ExceptionController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPut("exception")]
        public IActionResult Exception(ExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionType.ArgumentException:
                    throw new ArgumentException("My data not found in data store.");
                case ExceptionType.ArgumentNullException:
                    throw new ArgumentNullException(nameof(exceptionType), "My data not found in data store.");
                case ExceptionType.DivideByZeroException:
                    int zero = 0;
                    _ = 100 / zero;
                    break;
                case ExceptionType.FormatException:
                    decimal price = 169.32m;
                    _ = string.Format("The cost is {0:Q2}.", price);
                    break;
                case ExceptionType.IndexOutOfRangeException:
                    var characters = new Char[] { 'a', 'b', 'c' };
                    _ = characters[10];
                    break;
                default:
                    break;
            }

            return new OkResponse("ok");

        }

        [HttpPut("exception/sql")]
        public async Task<IActionResult> ExceptionSql()
        {
            var data = await _blogService.GetBlogsSpAsync();

            return new OkResponse(data);
        }
    }
}
