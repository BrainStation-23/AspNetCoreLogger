using System.Net;

namespace WebApp.Core.Models
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
        public string Body { get; set; }
        public string Headers { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
