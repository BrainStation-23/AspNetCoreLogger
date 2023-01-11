using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    public static class AuditObjectInit
    {
        public static object GetAuditObject()
        {
            string json = "{\r\n  \"userId\": 0,\r\n  \"traceId\": \"400001b9-0000-fc00-b63f-84710c7967bb\",\r\n  \"type\": \"Update\",\r\n  \"schemaName\": \"dbo\",\r\n  \"tableName\": \"BlogEntity\",\r\n  \"dateTime\": \"2023-01-09T19:39:49.5268842+06:00\",\r\n  \"primaryKey\": {\r\n    \"id\": 307\r\n  },\r\n  \"oldValues\": {\r\n    \"createdDateUtc\": \"2023-01-09T19:39:09.6025626+06:00\",\r\n    \"description\": \"My blog description\",\r\n    \"motto\": \"Blog Motto\",\r\n    \"name\": \"My Blog\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": null\r\n  },\r\n  \"newValues\": {\r\n    \"createdDateUtc\": \"0001-01-01T06:00:00+06:00\",\r\n    \"description\": \"Updated My blog description\",\r\n    \"motto\": \"Updated Blog Motto\",\r\n    \"name\": \"Updated My Blog\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": \"2023-01-09T19:39:49.4912424+06:00\"\r\n  },\r\n  \"affectedColumns\": [\r\n    \"CreatedDateUtc\",\r\n    \"Description\",\r\n    \"Motto\",\r\n    \"Name\",\r\n    \"UpdatedDateUtc\"\r\n  ],\r\n  \"createdBy\": 0,\r\n  \"createdDateUtc\": \"2023-01-09T13:39:49.5268857Z\",\r\n  \"updatedBy\": 0,\r\n  \"updatedDateUtc\": null\r\n}";
            return json.ToModel<object>();
        }
    }
}
