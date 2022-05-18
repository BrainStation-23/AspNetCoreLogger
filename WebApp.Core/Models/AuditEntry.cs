using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using WebApp.Core.Enums;

namespace WebApp.Core.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }


        public EntityEntry Entry { get; }

        public long UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumnNames { get; } = new List<string>();
        public IDictionary<string, object> Changes { get; set; } = new Dictionary<string, object>();
    }

}
