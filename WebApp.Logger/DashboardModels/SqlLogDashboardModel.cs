using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Logger.DashboardModels
{
    public class SqlLogDashboardModel
    {
        public string ApplicationName { set; get; }
        public string IpAddress { set; get; }
        public string Url { set; get; }
        public string TraceId { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
        public string MethodName { set; get; }
        public string QueryType { set; get; }
        public string Query { set; get; }
        public string Duration { set; get; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
