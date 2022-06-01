using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Helpers.Attributes
{
    public class RouteFilterAttribute : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteFilterAttribute(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var route = new RouteLogModel();
            var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            var actionDescriptorRouteValues = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.RouteValues;

            if (actionDescriptorRouteValues.ContainsKey("area"))
            {
                var area = actionDescriptorRouteValues["area"];
                if (area != null)
                {
                    route.Area = Convert.ToString(area);
                }
            }

            var request = context.HttpContext.Request;
            RequestHeaders header = request.GetTypedHeaders();

            route.ControllerName = controllerName;
            route.ActionName = actionName;
            //route.SessionId = context.HttpContext.Session.Id;
            route.UrlReferrer = header.Referer?.AbsoluteUri;
            if (_httpContextAccessor.HttpContext != null)
                route.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            await next();

            // _auditRepository.InsertAuditLogs(objaudit);
        }

        public async Task<string> BodyToString(HttpRequest request)
        {
            string body = string.Empty;
            request.EnableBuffering();

            // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;
            }

            return body;
        }
    }
}
