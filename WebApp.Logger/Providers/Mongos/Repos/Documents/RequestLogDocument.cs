using MongoDB.Bson;
using System;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;

namespace WebApp.Logger.Providers.Mongos.Repos.Documents
{

    [BsonCollection("RequestLog")]
    public class RequestLogDocument : RequestModel, IDocument
    {
        public RequestLogDocument()
        {
            Id = ObjectId.GenerateNewId();
            CreatedDateUtc = Id.CreationTime;
        }
        public ObjectId Id { get; set; }
    }
}
