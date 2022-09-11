using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.CosmosDbs;

namespace WebApp.Logger.Loggers.Providers.CosmosDbs.Items
{

    public class AuditLogItem : AuditModel, IItem
    {
        public new long CreatedBy { get; set; }
        public string Id => Guid.NewGuid().ToString();
        public DateTime CreatedDateUtc => DateTime.UtcNow;
    }
}
