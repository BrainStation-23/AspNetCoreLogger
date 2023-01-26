using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Logger.DashboardModels
{
    public class RequestLogDashboardModel
    {
        public string ApplicationName { get; set; }
        public string Url { get; set; }
        public string IpAddress { get; set; }
        public string TraceId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RequestMethod { get; set; }
        public double Duration { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}