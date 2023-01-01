using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using WebApp.Common.Serialize;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{

    [BsonCollection("AuditLog")]
    public class AuditLogDocument : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public AuditLogDocument()
        {
            Id = ObjectId.GenerateNewId();
            CreatedDateUtc = Id.CreationTime;
        }

        public long UserId { get; set; }
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public DateTime? DateTime { get; set; }
        public Dictionary<string, dynamic> PrimaryKey { get; set; } //Dictionary
        public Dictionary<string, dynamic> OldValues { get; set; } //Dictionary
        public Dictionary<string, dynamic> NewValues { get; set; } //Dictionary
        public Dictionary<string, dynamic> AffectedColumns { get; set; } //Dictionary
        public long CreatedBy { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
    }
}
