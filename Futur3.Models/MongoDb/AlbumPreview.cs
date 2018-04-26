using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Futur3.Models.DTO
{
    public class AlbumPreview
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("albumId")]
        public int AlbumId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("webSite")]
        public string WebSite { get; set; }
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("lat")]
        public string Lat { get; set; }
        [BsonElement("lng")]
        public string Lng { get; set; }
        [BsonElement("photoCount")]
        public int PhotoCount { get; set; }
        [BsonElement("randomThumbnailUrl")]
        public string RandomThumbnailUrl { get; set; }
        [BsonElement("_createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
