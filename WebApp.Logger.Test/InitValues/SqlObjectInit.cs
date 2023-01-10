using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    public static class SqlObjectInit
    {
        public static object GetSqlObject()
        {
            string json = "{\r\n  \"userId\": null,\r\n  \"applicationName\": \"\",\r\n  \"ipAddress\": \"::1\",\r\n  \"version\": \"\",\r\n  \"host\": \"localhost:44373\",\r\n  \"url\": \"https://localhost:44373/api/Blog/edit/307\",\r\n  \"source\": \"Query\",\r\n  \"scheme\": \"https\",\r\n  \"traceId\": \"400001b9-0000-fc00-b63f-84710c7967bb\",\r\n  \"protocol\": \"HTTP/2\",\r\n  \"urlReferrer\": \"\",\r\n  \"area\": \"\",\r\n  \"controllerName\": \"Blog\",\r\n  \"actionName\": \"Edit\",\r\n  \"className\": \"\",\r\n  \"methodName\": \"\",\r\n  \"connection\": {\r\n    \"database\": \"DotnetLoggerWrapper\",\r\n    \"dataSource\": \"localhost\",\r\n    \"serverVersion\": \"15.00.2000\",\r\n    \"connectionId\": \"e4e77e9d-67a3-4ffd-9f6d-4e30e197ac3c\",\r\n    \"connectionTimeout\": 15\r\n  },\r\n  \"command\": {\r\n    \"commandTimeout\": 30,\r\n    \"commandType\": \"Text\"\r\n  },\r\n  \"event\": {\r\n    \"name\": \"Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted\"\r\n  },\r\n  \"queryType\": \"Text\",\r\n  \"query\": \"SET NOCOUNT ON;\\r\\nUPDATE [Blogs] SET [CreatedBy] = @p0, [CreatedDateUtc] = @p1, [Description] = @p2, [Motto] = @p3, [Name] = @p4, [UpdatedBy] = @p5, [UpdatedDateUtc] = @p6\\r\\nWHERE [Id] = @p7;\\r\\nSELECT @@ROWCOUNT;\\r\\n\\r\\n\",\r\n  \"response\": null,\r\n  \"duration\": 14.6849,\r\n  \"message\": null,\r\n  \"createdDateUtc\": \"2023-01-09T13:39:49.5655732Z\"\r\n}";
            return json.ToModel<object>();
        }
    }
}
