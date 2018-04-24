﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace Futur3.Models.MongoDb
{
    public class Album
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("externalId")]
        [JsonProperty("id")]
        public int ExternalId { get; set; }

        [BsonElement("userId")]
        public int UserId { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("_createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
