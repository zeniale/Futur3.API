using MongoDB.Driver;

using Futur3.Models;
using Futur3.Models.DTO;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public static class IndexesGenerator
    {
        public static void CreateIndexes(ApplicationSettings applicationSettings)
        {

            IMongoClient client = new MongoClient(applicationSettings.MongoDbSettings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(applicationSettings.MongoDbSettings.DatabaseName);

            IMongoCollection<Album> albumsCollection = database.GetCollection<Album>(applicationSettings.MongoDbSettings.AlbumCollection);
            albumsCollection.Indexes.CreateOne(Builders<Album>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });

            IMongoCollection<User> usersCollection = database.GetCollection<User>(applicationSettings.MongoDbSettings.UserCollection);
            usersCollection.Indexes.CreateOne(Builders<User>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });

            IMongoCollection<Photo> photosCollection = database.GetCollection<Photo>(applicationSettings.MongoDbSettings.PhotoCollection);
            photosCollection.Indexes.CreateOne(Builders<Photo>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });

            IMongoCollection<AlbumPreview> albumsPreviewCollection = database.GetCollection<AlbumPreview>(applicationSettings.MongoDbSettings.AlbumPreviewCollection);
            albumsPreviewCollection.Indexes.CreateOne(Builders<AlbumPreview>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });
        }
    }
}
