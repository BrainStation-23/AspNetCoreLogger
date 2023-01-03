using System;
using System.Collections.Generic;
using System.Net;
using WebApp.Common.Serialize;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Providers.Mongos
{
    public static class MongoDocumentMapper
    {
        public static AuditLogDocument ToDocument(this AuditModel model)
        {
            return new AuditLogDocument
            {
                UserId = model.UserId,
                Type = model.Type,
                TableName = model.TableName,
                DateTime = model.DateTime,
                PrimaryKey = model.PrimaryKey.IdentifyNestedObjects(),
                OldValues = model.OldValues.IdentifyNestedObjects(),
                NewValues = model.NewValues.IdentifyNestedObjects(),
                AffectedColumns = model.AffectedColumns.IdentifyNestedObjects(),
                CreatedBy = model.CreatedBy,
                TraceId = model.TraceId
            };
        }

        public static RequestLogDocument ToDocument(this RequestModel model)
        {
            return new RequestLogDocument
            {
                UserId = model.UserId,
                Application = model.Application,
                IpAddress = model.IpAddress,
                Version = model.Version,
                Host = model.Host,
                Url = model.Url,
                Source = model.Source,
                Form = model.Form,
                Body = model.Body.IdentifyNestedObjects(),
                Response = model.Response.IdentifyNestedObjects(),
                RequestHeaders = model.RequestHeaders.IdentifyNestedObjects(),
                ResponseHeaders = model.ResponseHeaders.IdentifyNestedObjects(),
                Scheme = model.Scheme,
                TraceId = model.TraceId,
                Proctocol = model.Proctocol,
                Area = model.Area,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                ExecutionDuration = model.ExecutionDuration,
                RoleId = model.RoleId,
                LanguageId = model.LanguageId,
                IsFirstLogin = model.IsFirstLogin,
                LoggedInDateTimeUtc = model.LoggedInDateTimeUtc,
                LoggedOutDateTimeUtc = model.LoggedInDateTimeUtc,
                LoginStatus = model.LoginStatus,
                PageAccessed = model.PageAccessed,
                SessionId = model.SessionId,
                UrlReferrer = model.UrlReferrer,
                StatusCode = model.StatusCode,
                AppStatusCode = model.AppStatusCode,
            };
        }

        public static SqlLogDocument ToDocument(this SqlModel model)
        {
            return new SqlLogDocument
            {
                ActionName= model.ActionName,
                ApplicationName= model.ApplicationName,
                Area= model.Area,
                ControllerName= model.ControllerName,
                ClassName= model.ClassName,
                Command= model.Command.IdentifyNestedObjects(),
                IpAddress= model.IpAddress,
                Connection= model.Connection.IdentifyNestedObjects(),
                Duration= model.Duration,
                Event= model.Event.IdentifyNestedObjects(),
                Host= model.Host,
                Message= model.Message,
                MethodName= model.MethodName,
                Protocol= model.Protocol,
                Query= model.Query,
                QueryType= model.QueryType,
                Response= model.Response,
                Scheme= model.Scheme,
                Source= model.Source,
                TraceId= model.TraceId,
                Url= model.Url,
                UrlReferrer= model.UrlReferrer,
                UserId= model.UserId,
                Version= model.Version
            };
        }

        public static ErrorLogDocument ToDocument(this ErrorModel model)
        {
            return new ErrorLogDocument
            {
                UserId = model.UserId,
                Application = model.Application,
                IpAddress = model.IpAddress,
                Version = model.Version,
                Host = model.Host,
                Url = model.Url,
                Source = model.Source,
                Form = model.Form,
                Body = model.Body.IdentifyNestedObjects(),
                Response = model.Response.IdentifyNestedObjects(),
                RequestHeaders = model.RequestHeaders.IdentifyNestedObjects(),
                ResponseHeaders = model.ResponseHeaders.IdentifyNestedObjects(),
                ErrorCode = model.ErrorCode,
                Scheme = model.Scheme,
                TraceId = model.TraceId,
                Proctocol = model.Proctocol,
                Errors = model.Errors,
                StatusCode = model.StatusCode,
                AppStatusCode = model.AppStatusCode,
                Message = model.Message,
                MessageDetails = model.MessageDetails,
                StackTrace = model.StackTrace,
                
            };
        }
    }
}
