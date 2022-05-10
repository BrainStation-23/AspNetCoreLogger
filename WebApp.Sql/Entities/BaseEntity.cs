using System;
using WebApp.Core.Sqls;

namespace WebApp.Sql.Entities
{
    public class BaseEntity : MasterEntity
    {
        public long CreatedBy { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDateUtc { get; set; }
    }
}
