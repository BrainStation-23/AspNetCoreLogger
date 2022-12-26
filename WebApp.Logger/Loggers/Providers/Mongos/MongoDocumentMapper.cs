using System;
using System.Collections.Generic;
using System.Net;
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
                PrimaryKey = model.PrimaryKey,
                OldValues = model.OldValues,
                NewValues = model.NewValues,
                AffectedColumns = model.AffectedColumns,
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
                Body = model.Body,
                Response = model.Response,
                RequestHeaders = model.ResponseHeaders,
                ResponseHeaders = model.ResponseHeaders,
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
                Command= model.Command,
                IpAddress= model.IpAddress,
                Connection= model.Connection,
                Duration= model.Duration,
                Event= model.Event,
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
                Body = model.Body,
                Response = model.Response,
                RequestHeaders = model.RequestHeaders,
                ResponseHeaders = model.ResponseHeaders,
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
