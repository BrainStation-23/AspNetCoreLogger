using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebApp.Logger.Collections;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Responses
{
    internal class OkResponse : IActionResult
    {
        private readonly object _result;

        public OkResponse(object result)
        {
            _result = result;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (_result?.GetType().Name == typeof(Paging<>).Name)
            {
                var total = _result.GetPropValue("Total");
                var response = context.HttpContext.Response;
                response.Headers.Add("X-Total-Count", total.ToString());
            }

            var objectResult = new ObjectResult(_result)
            {
                StatusCode = StatusCodes.Status200OK,
                Value = new
                {
                    data = _result,
                    StatusCode = (int)HttpStatusCode.OK,
                    //AppStatusCode = ((int)HttpStatusCode.OK).ToAppStatusCode(),
                    IsSuccess = true
                }
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
