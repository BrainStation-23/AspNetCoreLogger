using System;
using System.Collections.Generic;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

namespace WebApp.Logger.Test.Extensions
{
    public static class SqlObjectInit
    {
        public static object GetSqlObject()
        {
            string json = "{\r\n  \"userId\": null,\r\n  \"applicationName\": \"\",\r\n  \"ipAddress\": \"::1\",\r\n  \"version\": \"\",\r\n  \"host\": \"localhost:44373\",\r\n  \"url\": \"https://localhost:44373/api/Blog/edit/307\",\r\n  \"source\": \"Query\",\r\n  \"scheme\": \"https\",\r\n  \"traceId\": \"400001b9-0000-fc00-b63f-84710c7967bb\",\r\n  \"protocol\": \"HTTP/2\",\r\n  \"urlReferrer\": \"\",\r\n  \"area\": \"\",\r\n  \"controllerName\": \"Blog\",\r\n  \"actionName\": \"Edit\",\r\n  \"className\": \"\",\r\n  \"methodName\": \"\",\r\n  \"connection\": {\r\n    \"database\": \"DotnetLoggerWrapper\",\r\n    \"dataSource\": \"localhost\",\r\n    \"serverVersion\": \"15.00.2000\",\r\n    \"connectionId\": \"e4e77e9d-67a3-4ffd-9f6d-4e30e197ac3c\",\r\n    \"connectionTimeout\": 15\r\n  },\r\n  \"command\": {\r\n    \"commandTimeout\": 30,\r\n    \"commandType\": \"Text\"\r\n  },\r\n  \"event\": {\r\n    \"name\": \"Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted\"\r\n  },\r\n  \"queryType\": \"Text\",\r\n  \"query\": \"SET NOCOUNT ON;\\r\\nUPDATE [Blogs] SET [CreatedBy] = @p0, [CreatedDateUtc] = @p1, [Description] = @p2, [Motto] = @p3, [Name] = @p4, [UpdatedBy] = @p5, [UpdatedDateUtc] = @p6\\r\\nWHERE [Id] = @p7;\\r\\nSELECT @@ROWCOUNT;\\r\\n\\r\\n\",\r\n  \"response\": null,\r\n  \"duration\": 14.6849,\r\n  \"message\": null,\r\n  \"createdDateUtc\": \"2023-01-09T13:39:49.5655732Z\"\r\n}";
            return json.ToModel<object>();
        }

        public static string GetSqlObjectStringWithHeaderAndFooter()
        {
            return "01-19-2023 12:16:43 +06 [sql] {\r\n  \"userId\": null,\r\n  \"applicationName\": \"\",\r\n  \"ipAddress\": \"::1\",\r\n  \"version\": \"\",\r\n  \"Host\": \"localhost:5001\",\r\n  \"url\": \"https://localhost:5001/api/Blog/AddDummyBlogs/50\",\r\n  \"source\": \"Query\",\r\n  \"scheme\": \"https\",\r\n  \"traceId\": \"0HMNPSIFGLMSJ:0000000B\",\r\n  \"protocol\": \"HTTP/2\",\r\n  \"urlReferrer\": \"\",\r\n  \"area\": \"\",\r\n  \"controllerName\": \"Blog\",\r\n  \"actionName\": \"AddDummyBlogDetail\",\r\n  \"className\": \"\",\r\n  \"methodName\": \"\",\r\n  \"connection\": {\r\n    \"database\": \"DotnetLoggerWrapper\",\r\n    \"dataSource\": \"localhost\",\r\n    \"serverVersion\": \"15.00.2000\",\r\n    \"connectionId\": \"f9f01905-2df7-4d55-b92f-b60eb23da913\",\r\n    \"connectionTimeout\": 15\r\n  },\r\n  \"command\": {\r\n    \"commandTimeout\": 30,\r\n    \"commandType\": \"Text\"\r\n  },\r\n  \"event\": {\r\n    \"id\": 20101,\r\n    \"name\": \"Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted\"\r\n  },\r\n  \"queryType\": \"Text\",\r\n  \"query\": \"SELECT TOP(1) [b].[Id], [b].[CreatedBy], [b].[CreatedDateUtc], [b].[Description], [b].[Motto], [b].[Name], [b].[UpdatedBy], [b].[UpdatedDateUtc]\\r\\nFROM [Blogs] AS [b]\\r\\nWHERE [b].[Id] = @__p_0\",\r\n  \"response\": null,\r\n  \"duration\": 55.0595,\r\n  \"message\": null,\r\n  \"createdDateUtc\": \"2023-01-19T06:16:37.2675565Z\"\r\n}\r\n----------------------------------------------------------------------------------\r\n01-19-2023 12:16:43 +06 [sql] {\r\n  \"userId\": null,\r\n  \"applicationName\": \"\",\r\n  \"ipAddress\": \"::1\",\r\n  \"version\": \"\",\r\n  \"host\": \"localhost:5001\",\r\n  \"url\": \"https://localhost:5001/api/Blog/AddDummyBlogs/50\",\r\n  \"source\": \"Connection\",\r\n  \"scheme\": \"https\",\r\n  \"traceId\": \"0HMNPSIFGLMSJ:0000000B\",\r\n  \"protocol\": \"HTTP/2\",\r\n  \"urlReferrer\": \"\",\r\n  \"area\": \"\",\r\n  \"controllerName\": \"\",\r\n  \"actionName\": \"\",\r\n  \"className\": \"\",\r\n  \"methodName\": \"\",\r\n  \"connection\": {\r\n    \"database\": \"DotnetLoggerWrapper\",\r\n    \"dataSource\": \"localhost\",\r\n    \"connectionId\": \"f9f01905-2df7-4d55-b92f-b60eb23da913\",\r\n    \"connectionTimeout\": 15\r\n  },\r\n  \"command\": {\r\n    \"commandTimeout\": 0,\r\n    \"commandType\": \"\"\r\n  },\r\n  \"event\": {\r\n    \"id\": 20000,\r\n    \"name\": \"Microsoft.EntityFrameworkCore.Database.Connection.ConnectionOpening\"\r\n  },\r\n  \"queryType\": \"\",\r\n  \"query\": \"\",\r\n  \"response\": null,\r\n  \"duration\": 0.0,\r\n  \"message\": null,\r\n  \"createdDateUtc\": \"2023-01-19T06:16:37.3485516Z\"\r\n}\r\n----------------------------------------------------------------------------------";
        }

        public static List<SqlModel> GetDemoSqlModels()
        {
            List<SqlModel> sqlModels = new()
            {
                new(){ Host = "localhost:5001", ActionName = "Post",ApplicationName = "WebApp",CreatedDateUtc = DateTime.UtcNow},
                new(){ Host = "localhost:5002", ActionName = "Put",ApplicationName = "WebApp6",CreatedDateUtc = DateTime.UtcNow},
                new(){ Host = "localhost:5003", ActionName = "Post",ApplicationName = "WebApp7",CreatedDateUtc = DateTime.UtcNow},
            };

            return sqlModels;
        }
    }
}
