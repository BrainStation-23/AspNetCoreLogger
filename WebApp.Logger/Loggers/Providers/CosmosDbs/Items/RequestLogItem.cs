using Newtonsoft.Json;
using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.CosmosDbs;

namespace WebApp.Logger.Loggers.Providers.CosmosDbs.Items
{
    public class RequestLogItem : RequestModel, IItem
    {
        public long CreatedBy { get; set; }
        public string Id => Guid.NewGuid().ToString();
        public DateTime CreatedDateUtc => DateTime.UtcNow;
        public string LogType => "request";
    }
}
