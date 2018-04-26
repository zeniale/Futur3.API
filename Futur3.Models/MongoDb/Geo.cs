using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Futur3.Models.MongoDb
{
    public class Geo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("lat")]
        public string Lat { get; set; }
        [BsonElement("lng")]
        public string Lng { get; set; }
    }
}
