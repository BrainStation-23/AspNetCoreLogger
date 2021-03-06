using System.Net;

namespace WebApp.Core.Models
{
    public class RequestModel
    {
        public long? UserId { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Form { get; set; }
        public string Body { get; set; }
        public string Response { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
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
    }
}
