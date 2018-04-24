using MongoDB.Bson.Serialization.Attributes;

namespace Futur3.Models.MongoDb
{
    public class Company
    {

        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("catchPrase")]
        public string CatchPhrase { get; set; }
        [BsonElement("bs")]
        public string Bs { get; set; }
    }
}
