using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

using Futur3.Models.MongoDb;
using Futur3.Models;

namespace Futur3.Infrastructure.MongoDb
{
    public class AlbumsRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Album> _albumsCollection;

        private readonly IOptions<ApplicationSettings> _applicationSettings;

        public AlbumsRepository(IOptions<ApplicationSettings> applicationSettings)
        {
            this._applicationSettings = applicationSettings;
            this._client = new MongoClient(this._applicationSettings.Value.MongoDbSettings.ConnectionString);
            this._database = this._client.GetDatabase(this._applicationSettings.Value.MongoDbSettings.DatabaseName);
            this._albumsCollection = this._database.GetCollection<Album>(this._applicationSettings.Value.MongoDbSettings.AlbumCollection);
        }

        public async Task<List<Album>> GetAlbumsListAsync()
        {
            List<Album> albums = await _albumsCollection.Find(new BsonDocument()).ToListAsync();
            if (albums.Count == 0)
            {
                List<Album> remoteAlbums = await this.GetRemoteAlbumsAsync();
                List<Album> insertedAlbums = await this.InsertAlbumsAsync(remoteAlbums);
                return insertedAlbums;
            }
            return albums;
        }

        private async Task<List<Album>> GetRemoteAlbumsAsync()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(this._applicationSettings.Value.RemoteDataServiceSettings.AlbumsUrl))
            using (HttpContent content = response.Content)
            {
                string stringResult = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Album>>(stringResult);
            }
        }

        public async Task<Album> InsertAlbumAsync(Album album)
        {
            album.CreatedAt = DateTime.UtcNow;
            await _albumsCollection.InsertOneAsync(album);
            return album;
        }

        public async Task<List<Album>> InsertAlbumsAsync(List<Album> albums)
        {
            DateTime createdAt = DateTime.UtcNow;
            foreach (Album album in albums)
            {
                album.CreatedAt = createdAt;
            }
            await _albumsCollection.InsertManyAsync(albums);
            return albums;
        }

        public async Task<bool> UpdateUserAsync(ObjectId id, string udateFieldName, string updateFieldValue)
        {
            var filter = Builders<Album>.Filter.Eq("_id", id);
            var update = Builders<Album>.Update.Set(udateFieldName, updateFieldValue);

            var result = await _albumsCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<List<Album>> GetAlbumsByExternalIdAsync(int externaldId)
        {
            var filter = Builders<Album>.Filter.Eq(nameof(Album.Id), externaldId);
            var result = await _albumsCollection.Find(filter).ToListAsync();

            return result;
        }

        public async Task<bool> DeleteAlbumByIdAsync(ObjectId id)
        {
            var filter = Builders<Album>.Filter.Eq("_id", id);
            var result = await _albumsCollection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<long> DeleteAllAlbumsAsync()
        {
            var filter = new BsonDocument();
            var result = await _albumsCollection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }
    }
}
