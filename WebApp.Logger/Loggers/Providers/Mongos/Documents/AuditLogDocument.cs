using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{

    [BsonCollection("AuditLog")]
    public class AuditLogDocument : AuditModel, IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        public AuditLogDocument()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}
