using Microsoft.AspNetCore.Mvc;
using WebApp.Logger.Loggers.Providers.CosmosDbs.Items;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Providers.CosmosDbs
{
    public static class CosmosItemMapper
    {
        public static AuditLogItem ToItem(this AuditModel model)
        {
            return new AuditLogItem
            {
                UserId = model.UserId,
                Type = model.Type,
                TableName = model.TableName,
                DateTime = model.DateTime,
                PrimaryKey = model.PrimaryKey,
                OldValues = model.OldValues,
                NewValues = model.NewValues,
                AffectedColumns = model.AffectedColumns,
                CreatedBy = model.CreatedBy,
                TraceId = model.TraceId,
                ApplicationName = model.ApplicationName,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName
            };
        }

        public static SqlLogItem ToItem(this SqlModel model)
        {
            return new SqlLogItem
            {
                UserId = model.UserId,
                ApplicationName = model.ApplicationName,
                IpAddress = model.IpAddress,
                Version = model.Version,
                Host = model.Host,
                Url = model.Url,
                Source = model.Source,
                Scheme = model.Scheme,
                TraceId = model.TraceId,
                Protocol = model.Protocol,
                UrlReferrer = model.UrlReferrer,
                Area = model.Area,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                ClassName = model.ClassName,
                MethodName = model.MethodName,
                Connection = model.Connection,
                Command = model.Command,
                Event = model.Event,
                QueryType = model.QueryType,
                Query = model.Query,
                Response = model.Response,
                Duration = model.Duration,
                Message = model.Message,
            };
        }

        public static ErrorLogItem ToItem(this ErrorModel model)
        {
            return new ErrorLogItem
            {
                Application = model.Application,
                AppStatusCode = model.AppStatusCode,
                IpAddress = model.IpAddress,
                Body = model.Body,
                CreatedBy = 0,
                ErrorCode = model.ErrorCode,
                Errors = model.Errors,
                Form = model.Form,
                Host = model.Host,
                Message = model.Message,
                MessageDetails = model.MessageDetails,
                Proctocol = model.Proctocol,
                RequestHeaders = model.RequestHeaders,
                Response = model.Response,
                ResponseHeaders = model.ResponseHeaders,
                Scheme = model.Scheme,
                Source = model.Source,
                StackTrace = model.StackTrace,
                StatusCode = model.StatusCode,
                TraceId = model.TraceId,
                Url = model.Url,
                UserId = model.UserId,
                Version = model.Version,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                Duration = model.Duration,
                RequestMethod = model.RequestMethod
            };
        }


        public static RequestLogItem ToItem(this RequestModel model)
        {
            return new RequestLogItem
            {
                Application = model.Application,
                AppStatusCode = model.AppStatusCode,
                IpAddress = model.IpAddress,
                Body = model.Body,
                Form = model.Form,
                Host = model.Host,
                Proctocol = model.Proctocol,
                RequestHeaders = model.RequestHeaders,
                Response = model.Response,
                ResponseHeaders = model.ResponseHeaders,
                Scheme = model.Scheme,
                Source = model.Source,
                StatusCode = model.StatusCode,
                TraceId = model.TraceId,
                Url = model.Url,
                UserId = model.UserId,
                Version = model.Version,
                ActionName = model.ActionName,
                Area = model.Area,
                PageAccessed = model.PageAccessed,
                ControllerName = model.ControllerName,
                Duration = model.Duration,
                IsFirstLogin = model.IsFirstLogin,
                LanguageId = model.LanguageId,
                LoggedInDateTimeUtc = model.LoggedInDateTimeUtc,
                LoggedOutDateTimeUtc = model.LoggedOutDateTimeUtc,
                LoginStatus = model.LoginStatus,
                RoleId = model.RoleId,
                Session = model.Session,
                UrlReferrer = model.UrlReferrer,
                RequestMethod = model.RequestMethod,
                RequestLength = model.RequestLength,
                ResponseLength = model.ResponseLength,
                IsHttps = model.IsHttps,
                CorrelationId = model.CorrelationId,
                ApplicationName = model.ApplicationName,
            };
        }
    }
}
