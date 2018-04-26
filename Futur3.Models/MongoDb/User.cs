using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

using Futur3.Models.MongoDb.Interfaces;

namespace Futur3.Models.MongoDb
{
    public class User: IMongoDbDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("externalId")]
        [JsonProperty("id")]
        public int ExternalId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("address")]
        public Address Address { get; set; }
        [BsonElement("phone")]
        public string Phone { get; set; }
        [BsonElement("website")]
        public string Website { get; set; }
        [BsonElement("company")]
        public Company Company { get; set; }
        [BsonElement("_createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}
