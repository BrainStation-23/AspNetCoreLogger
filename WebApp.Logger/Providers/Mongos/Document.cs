using MongoDB.Bson;
using System;

namespace WebApp.Logger.Providers.Mongos
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime? CreatedDateUtc => Id.CreationTime;
    }
}
