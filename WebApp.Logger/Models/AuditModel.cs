using System;

namespace WebApp.Logger.Models
{
    public class AuditModel
    {
        public long UserId { get; set; }
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime? DateTime { get; set; }
        public object PrimaryKey { get; set; }
        public object OldValues { get; set; }
        public object NewValues { get; set; }
        public object AffectedColumns { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
    }
}
