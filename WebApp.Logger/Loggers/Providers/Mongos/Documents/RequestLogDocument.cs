using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{

    [BsonCollection("RequestLog")]
    public class RequestLogDocument : IDocument
    {
        public RequestLogDocument()
        {
            Id = ObjectId.GenerateNewId();
        }
        public ObjectId Id { get; set; }


        public long? UserId { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Form { get; set; }
        public Dictionary<string, dynamic> Body { get; set; } //Dictionary
        public Dictionary<string, dynamic> Response { get; set; }  //Dictionary
        public Dictionary<string, dynamic> RequestHeaders { get; set; } //Dictionary
        public Dictionary<string, dynamic> ResponseHeaders { get; set; } //Dictionary
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Proctocol { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public double ExecutionDuration { get; set; }
        public string RoleId { get; set; }
        public string LanguageId { get; set; }
        public string IsFirstLogin { get; set; }
        public string LoggedInDateTimeUtc { get; set; }
        public string LoggedOutDateTimeUtc { get; set; }
        public string LoginStatus { get; set; }
        public string PageAccessed { get; set; }
        public string SessionId { get; set; }
        public string UrlReferrer { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
