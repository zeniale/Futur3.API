using MongoDB.Bson;
using System;

namespace Futur3.Models.MongoDb.Interfaces
{
    public interface IMongoDbDocument
    {
        ObjectId Id { get; set; }
        int ExternalId { get; set; }

        DateTime CreatedAt { get; set; }
    }
}
