using System;
using WebApp.Logger.Models;

namespace WebApp.Logger.Providers.CosmosDbs.Repos.Items
{

    public class AuditLogItem : AuditModel, IItem
    {
        public new long CreatedBy { get; set; }
        public string Id => Guid.NewGuid().ToString();
        public new DateTime CreatedDateUtc => System.DateTime.UtcNow;
        public string LogType => "audit";
    }
}
