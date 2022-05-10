using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Helpers.Attributes
{
    public class RouteLogModel
    {
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RoleId { get; set; }
        public string LanguageId { get; set; }
        public string IpAddress { get; set; }
        public string IsFirstLogin { get; set; }
        public string LoggedInDateTimeUtc { get; set; }
        public string LoggedOutDateTimeUtc { get; set; }
        public string LoginStatus { get; set; }
        public string PageAccessed { get; set; }
        public string SessionId { get; set; }
        public string UrlReferrer { get; set; }
        public string UserId { get; set; }
    }
}
