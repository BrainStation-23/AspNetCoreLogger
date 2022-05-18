using System.Net;
using WebApp.Core.Sqls;

namespace WebApp.Entity.Entities.Logs
{
    public class ErrorLog : BaseEntity
    {
        public long? UserId { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
