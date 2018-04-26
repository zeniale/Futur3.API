using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

using Futur3.Models;
using Futur3.Models.MongoDb.Interfaces;

namespace Futur3.Infrastructure.MongoDb
{
    public class GenericRepository<T> where T : IMongoDbDocument
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;
        private readonly IOptions<ApplicationSettings> _applicationSettings;
        private readonly string _remoteCollectionUrl;

        public GenericRepository(
            IOptions<ApplicationSettings> applicationSettings,
            string collectionName,
            string remoteCollectionUrl)
        {
            this._applicationSettings = applicationSettings;
            this._client = new MongoClient(this._applicationSettings.Value.MongoDbSettings.ConnectionString);
            this._database = this._client.GetDatabase(this._applicationSettings.Value.MongoDbSettings.DatabaseName);
            this._collection = this._database.GetCollection<T>(collectionName);
            this._remoteCollectionUrl = remoteCollectionUrl;
        }

        public async Task<List<T>> GetListAsync()
        {
            await this.EnsureCollectionLoaded();
            return await _collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<T> InsertOneAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<List<T>> InsertManyAsync(List<T> entities)
        {
            DateTime createdAt = DateTime.UtcNow;
            foreach (T entity in entities)
            {
                entity.CreatedAt = createdAt;
            }
            await _collection.InsertManyAsync(entities);
            return entities;
        }

        public async Task<bool> UpdateOneAsync(ObjectId id, string udateFieldName, object updateFieldValue)
        {
            await this.EnsureCollectionLoaded();
            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set(udateFieldName, updateFieldValue);

            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<bool> UpdateOneByExternalIdAsync(int externalId, string udateFieldName, object updateFieldValue)
        {
            await this.EnsureCollectionLoaded();
            var filter = Builders<T>.Filter.Eq("externalId", externalId);
            var update = Builders<T>.Update.Set(udateFieldName, updateFieldValue);

            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<T> GetByExternalIdAsync(int externaldId)
        {
            await this.EnsureCollectionLoaded();
            var filter = Builders<T>.Filter.Eq("externalId", externaldId);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> DeleteOneByIdAsync(ObjectId id)
        {
            await this.EnsureCollectionLoaded();
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<long> DeleteAllAsync()
        {
            var filter = new BsonDocument();
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        protected async Task EnsureCollectionLoaded()
        {
            if (await _collection.CountAsync(new BsonDocument()) == 0)
            {
                List<T> remoteEntities = await this.GetRemoteEntitiesAsync();
                await this.InsertManyAsync(remoteEntities);
            }
        }

        private async Task<List<T>> GetRemoteEntitiesAsync()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(this._remoteCollectionUrl))
            using (HttpContent content = response.Content)
            {
                string stringResult = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(stringResult);
            }
        }
    }
}
