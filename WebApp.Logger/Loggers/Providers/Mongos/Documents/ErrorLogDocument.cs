using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{
    [BsonCollection("ErrorLog")]
    public class ErrorLogDocument : IDocument
    {
        public ErrorLogDocument() 
        {
            Id=ObjectId.GenerateNewId();
            CreatedDateUtc = Id.CreationTime;
        }
        public ObjectId Id { get; set; }


        public long? UserId { get; set; }
        public string Application { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Form { get; set; }
        public Dictionary<string,dynamic> Body { get; set; } //Dictionary
        public Dictionary<string, dynamic> Response { get; set; } //Dictionary
        public Dictionary<string, dynamic> RequestHeaders { get; set; } //Dictionary
        public Dictionary<string, dynamic> ResponseHeaders { get; set; } //Dictionary
        public string ErrorCode { get; set; }
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Proctocol { get; set; }
        public Dictionary<string, dynamic> Errors { get; set; } //Dictionary

        public HttpStatusCode StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public string Message { get; set; }
        public string MessageDetails { get; set; }
        public string StackTrace { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
