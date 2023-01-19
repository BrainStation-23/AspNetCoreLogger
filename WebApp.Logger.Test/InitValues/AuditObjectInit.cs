using System;
using System.Collections.Generic;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

namespace WebApp.Logger.Test.Extensions
{
    public static class AuditObjectInit
    {
        public static object GetAuditObject()
        {
            string json = "{\r\n  \"userId\": 0,\r\n  \"traceId\": \"400001b9-0000-fc00-b63f-84710c7967bb\",\r\n  \"type\": \"Update\",\r\n  \"schemaName\": \"dbo\",\r\n  \"tableName\": \"BlogEntity\",\r\n  \"dateTime\": \"2023-01-09T19:39:49.5268842+06:00\",\r\n  \"primaryKey\": {\r\n    \"id\": 307\r\n  },\r\n  \"oldValues\": {\r\n    \"createdDateUtc\": \"2023-01-09T19:39:09.6025626+06:00\",\r\n    \"description\": \"My blog description\",\r\n    \"motto\": \"Blog Motto\",\r\n    \"name\": \"My Blog\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": null\r\n  },\r\n  \"newValues\": {\r\n    \"createdDateUtc\": \"0001-01-01T06:00:00+06:00\",\r\n    \"description\": \"Updated My blog description\",\r\n    \"motto\": \"Updated Blog Motto\",\r\n    \"name\": \"Updated My Blog\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": \"2023-01-09T19:39:49.4912424+06:00\"\r\n  },\r\n  \"affectedColumns\": [\r\n    \"CreatedDateUtc\",\r\n    \"Description\",\r\n    \"Motto\",\r\n    \"Name\",\r\n    \"UpdatedDateUtc\"\r\n  ],\r\n  \"createdBy\": 0,\r\n  \"createdDateUtc\": \"2023-01-09T13:39:49.5268857Z\",\r\n  \"updatedBy\": 0,\r\n  \"updatedDateUtc\": null\r\n}";
            return json.ToModel<object>();
        }

        public static string GetAuditObjectStringWithHeaderAndFooter()
        {
            return "01-19-2023 12:16:43 +06 [audit] {\r\n  \"userId\": 0,\r\n  \"traceId\": \"0HMNPSIFGLMSJ:0000000B\",\r\n  \"type\": \"Create\",\r\n  \"schemaName\": \"dbo\",\r\n  \"tableName\": \"BlogEntity\",\r\n  \"dateTime\": \"2023-01-19T12:16:43.4809115+06:00\",\r\n  \"primaryKey\": {\r\n    \"id\": -9223372036854774807\r\n  },\r\n  \"oldValues\": {},\r\n  \"newValues\": {\r\n    \"createdDateUtc\": \"2023-01-19T12:16:36.3785524+06:00\",\r\n    \"description\": \"My blog description36cd6bc1-f60b-43ad-ae45-23dcc33cea82\",\r\n    \"motto\": \"Blog Motto\",\r\n    \"name\": \"My Blog36cd6bc1-f60b-43ad-ae45-23dcc33cea82\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": null\r\n  },\r\n  \"affectedColumns\": [],\r\n  \"createdBy\": 0,\r\n  \"createdDateUtc\": \"2023-01-19T06:16:43.4813699Z\",\r\n  \"updatedBy\": 0,\r\n  \"updatedDateUtc\": null\r\n}\r\n----------------------------------------------------------------------------------\r\n01-19-2023 12:16:43 +06 [audit] {\r\n  \"userId\": 0,\r\n  \"traceId\": \"0HMNPSIFGLMSJ:0000000B\",\r\n  \"type\": \"Create\",\r\n  \"schemaName\": \"dbo\",\r\n  \"tableName\": \"BlogEntity\",\r\n  \"dateTime\": \"2023-01-19T12:16:43.48157+06:00\",\r\n  \"primaryKey\": {\r\n    \"id\": -9223372036854774806\r\n  },\r\n  \"oldValues\": {},\r\n  \"newValues\": {\r\n    \"createdDateUtc\": \"2023-01-19T12:16:37.6250894+06:00\",\r\n    \"description\": \"My blog descriptionf6726c53-d79d-4181-872c-511f8b678bbc\",\r\n    \"motto\": \"Blog Motto\",\r\n    \"name\": \"My Blogf6726c53-d79d-4181-872c-511f8b678bbc\",\r\n    \"updatedBy\": 0,\r\n    \"updatedDateUtc\": null\r\n  },\r\n  \"affectedColumns\": [],\r\n  \"createdBy\": 0,\r\n  \"createdDateUtc\": \"2023-01-19T06:16:43.4815711Z\",\r\n  \"updatedBy\": 0,\r\n  \"updatedDateUtc\": null\r\n}\r\n----------------------------------------------------------------------------------";
        }

        public static List<AuditModel> GetDemoAuditModels()
        {
            List<AuditModel> auditModels = new()
            {
                new(){
                    NewValues =new List<Tuple<string,string> >(){
                     Tuple.Create("Name","New Blog"),
                     Tuple.Create("Description","New Description"),
                },
                    AffectedColumns=new List<string>(){ "Name", "Description" },
                    TableName="BlogModel"
                },
                new(){
                    NewValues =new List<Tuple<string,string> >(){
                     Tuple.Create("Name","New Blog"),
                     Tuple.Create("Description","New Description"),
                },
                    AffectedColumns=new List<string>(){ "Name", "Description" },
                    TableName="BlogModel"
                },
            };

            return auditModels;
        }
    }
}
