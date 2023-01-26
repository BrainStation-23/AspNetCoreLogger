using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Logger.DashboardModels
{
    public class ErrorLogDashboardModel
    {
        public string ApplicationName { get; set; }
        public string IpAddress { get; set; }
        public string Url { get; set; }
        public string ErrorCode { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public double Duration { get; set; }
        public string RequestMethod { get; set; }
    }
}
