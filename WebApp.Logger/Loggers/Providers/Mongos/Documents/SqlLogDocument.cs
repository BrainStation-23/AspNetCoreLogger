using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{
    [BsonCollection("SqlLog")]
    public class SqlLogDocument : IDocument
    {
        public SqlLogDocument() 
        {
            Id = ObjectId.GenerateNewId();
            CreatedDateUtc = Id.CreationTime;
        }
        public ObjectId Id { get; set; }

        public long? UserId { get; set; }
        public string ApplicationName { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Protocol { get; set; }
        public string UrlReferrer { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }

        public Dictionary<string,dynamic> Connection { get; set; } //Dictionary
        public Dictionary<string, dynamic> Command { get; set; } //Dictionary
        public Dictionary<string, dynamic> Event { get; set; } //Dictionary
        public string QueryType { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
        public double Duration { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDateUtc { get; set; } = DateTime.UtcNow;
    }
}
