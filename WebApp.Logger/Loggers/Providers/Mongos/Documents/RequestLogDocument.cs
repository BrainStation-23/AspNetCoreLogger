using MongoDB.Bson;
using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{

    [BsonCollection("RequestLog")]
    public class RequestLogDocument : RequestModel, IDocument
    {
        public ObjectId Id => ObjectId.GenerateNewId();
        public DateTime CreatedDateUtc => Id.CreationTime;
    }
}
