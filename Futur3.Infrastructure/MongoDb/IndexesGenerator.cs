using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

using Futur3.Models;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public class IndexesGenerator
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Album> _albumsCollection;
        private IMongoCollection<User> _usersCollection;

        private readonly IOptions<ApplicationSettings> _applicationSettings;

        public IndexesGenerator(IOptions<ApplicationSettings> applicationSettings)
        {
            this._applicationSettings = applicationSettings;
            this._client = new MongoClient(this._applicationSettings.Value.MongoDbSettings.ConnectionString);
            this._database = this._client.GetDatabase(this._applicationSettings.Value.MongoDbSettings.DatabaseName);
        }

        public async Task CreateIndexes()
        {
            this._albumsCollection = this._database.GetCollection<Album>(this._applicationSettings.Value.MongoDbSettings.AlbumCollection);
            await this._albumsCollection.Indexes.CreateOneAsync(Builders<Album>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });

            this._usersCollection = this._database.GetCollection<User>(this._applicationSettings.Value.MongoDbSettings.UserCollection);
            await this._usersCollection.Indexes.CreateOneAsync(Builders<User>.IndexKeys.Ascending(_ => _.CreatedAt), new CreateIndexOptions { ExpireAfter = new System.TimeSpan(0, 2, 0) });
        }
    }
}
