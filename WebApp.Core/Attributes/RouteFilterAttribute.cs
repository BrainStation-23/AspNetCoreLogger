using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApp.Helpers.Attributes
{
    public class RouteFilterAttribute : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteFilterAttribute(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
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

            route.LanguageId = Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.LangId));
            route.UserId = Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.UserId));
            route.RoleId = Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            route.IsFirstLogin = Convert.ToString(context.HttpContext.Session.GetString(AllSessionKeys.IsFirstLogin));
            route.PageAccessed = Convert.ToString(context.HttpContext.Request.Path);
            route.LoginStatus = "A";
            route.ControllerName = controllerName;
            route.ActionName = actionName;
            route.SessionId = context.HttpContext.Session.Id;
            route.UrlReferrer = header.Referer?.AbsoluteUri;
            if (_httpContextAccessor.HttpContext != null)
                route.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

            // _auditRepository.InsertAuditLogs(objaudit);
        }
    }

    public static class AllSessionKeys
    {
        public const string RoleId = "Portal.RoleId";
        public const string UserName = "Portal.UserName";
        public const string LangId = "Portal.LangId";
        public const string UserId = "Portal.UserId";
        public const string IsFirstLogin = "Portal.IsFirstLogin";
    }
}
