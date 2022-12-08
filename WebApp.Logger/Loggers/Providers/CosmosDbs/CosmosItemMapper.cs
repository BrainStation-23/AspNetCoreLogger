using WebApp.Logger.Loggers.Providers.CosmosDbs.Items;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Providers.CosmosDbs
{
    public static class CosmosItemMapper
    {
        public static AuditLogItem ToItem(this AuditModel model)
        {
            return new AuditLogItem
            {
                UserId = model.UserId,
                Type = model.Type,
                TableName = model.TableName,
                DateTime = model.DateTime,
                PrimaryKey = model.PrimaryKey,
                OldValues = model.OldValues,
                NewValues = model.NewValues,
                AffectedColumns = model.AffectedColumns,
                CreatedBy = model.CreatedBy,
                TraceId = model.TraceId
            };
        }

        public static SqlLogItem ToItem(this SqlModel model)
        {
            return new SqlLogItem
            {
               
            };
        }

        public static ErrorLogItem ToItem(this ErrorModel model)
        {
            return new ErrorLogItem
            {
                
            };
        }


        public static RequestLogItem ToItem(this RequestModel model)
        {
            return new RequestLogItem
            {
                
            };
        }
    }
}
