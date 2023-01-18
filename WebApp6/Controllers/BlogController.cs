using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Service;
using WebApp6.ViewModels.Enums;

namespace WebApp6.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ExceptionController
    {
        private readonly IBlogService _blogService;

        public ExceptionController(IBlogService blogService, IMapper mapper)
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
                    break;
                case ExceptionType.ArgumentNullException:
                    throw new ArgumentNullException("My data not found in data store.");
                    break;
                case ExceptionType.DivideByZeroException:
                    int zero = 0;
                    var number = 100 / zero;
                    break;
                case ExceptionType.FormatException:
                    decimal price = 169.32m;
                    var message = string.Format("The cost is {0:Q2}.", price);
                    break;
                case ExceptionType.IndexOutOfRangeException:
                    var characters = new Char[] { 'a', 'b', 'c' };
                    var char10 = characters[10];
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
