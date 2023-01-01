using System;
using System.Collections.Generic;
using System.Net;

namespace WebApp.Logger.Models
{
    public class ErrorModel
    {
        public long? UserId { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Form { get; set; }
        public object Body { get; set; } //object
        public object Response { get; set; } //object
        public object RequestHeaders { get; set; } //object
        public object ResponseHeaders { get; set; } //object
        public string ErrorCode { get; set; }
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Proctocol { get; set; }
        public IEnumerable<object> Errors { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public string Message { get; set; }
        public string MessageDetails { get; set; }
        public string StackTrace { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
