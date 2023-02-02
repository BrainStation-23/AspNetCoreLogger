using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Logger.DashboardModels
{
    public class TopRequestDashboardModel
    {
        public string RequestMethod { get; set; }
        public string Url { get; set; }
        public string ApplicationName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public long Frequency { get; set; }
    }
}
