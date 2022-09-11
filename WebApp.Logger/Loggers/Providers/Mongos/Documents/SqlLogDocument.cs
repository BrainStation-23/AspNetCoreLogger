using MongoDB.Bson;
using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Loggers.Providers.Mongos
{
    [BsonCollection("SqlLog")]
    public class SqlLogDocument : SqlModel, IDocument
    {
        public ObjectId Id => ObjectId.GenerateNewId();
        public DateTime CreatedDateUtc => Id.CreationTime;
    }
}
