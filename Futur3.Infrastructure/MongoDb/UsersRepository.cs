using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

using Futur3.Models;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public class UsersRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<User> _usersCollection;

        private readonly IOptions<ApplicationSettings> _applicationSettings;

        public UsersRepository(IOptions<ApplicationSettings> applicationSettings)
        {
            this._applicationSettings = applicationSettings;
            this._client = new MongoClient(this._applicationSettings.Value.MongoDbSettings.ConnectionString);
            this._database = this._client.GetDatabase(this._applicationSettings.Value.MongoDbSettings.DatabaseName);
            this._usersCollection = this._database.GetCollection<User>(this._applicationSettings.Value.MongoDbSettings.UserCollection);
        }

        public async Task<List<User>> GetAllUsersListAsync()
        {
            List<User> users = await _usersCollection.Find(new BsonDocument()).ToListAsync();
            if (users.Count == 0)
            {
                List<User> remoteUsers = await this.GetRemoteUsersAsync();
                List<User> insertedUsers = await this.InsertUsersAsync(remoteUsers);
                return insertedUsers;
            }
            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            User user = await this.GetUserAsync(userId);
            if (user == null)
            {
                User remoteUser = await this.GetRemoteUserAsync(userId);
                User insertedUser = await this.InsertUserAsync(remoteUser);
                return insertedUser;
            }
            return user;
        }

        private async Task<List<User>> GetRemoteUsersAsync()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(this._applicationSettings.Value.RemoteDataServiceSettings.UsersUrl))
            using (HttpContent content = response.Content)
            {
                string stringResult = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(stringResult);
            }
        }

        private async Task<User> GetRemoteUserAsync(int userId)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync($"{this._applicationSettings.Value.RemoteDataServiceSettings.UsersUrl}?id=${userId}"))
            using (HttpContent content = response.Content)
            {
                string stringResult = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(stringResult);
            }
        }

        public async Task<User> InsertUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<List<User>> InsertUsersAsync(List<User> users)
        {
            DateTime createdAt = DateTime.UtcNow;
            foreach (User user in users)
            {
                user.CreatedAt = createdAt;
            }
            await _usersCollection.InsertManyAsync(users);
            return users;
        }

        public async Task<bool> UpdateUserAsync(ObjectId id, string udateFieldName, string updateFieldValue)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            var update = Builders<User>.Update.Set(udateFieldName, updateFieldValue);

            var result = await _usersCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount != 0;
        }

        public async Task<User> GetUserByExternalIdAsync(int externaldId)
        {
            var filter = Builders<User>.Filter.Eq(nameof(User.Id), externaldId);
            var result = await _usersCollection.Find(filter).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> DeleteUserByIdAsync(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            var result = await _usersCollection.DeleteOneAsync(filter);
            return result.DeletedCount != 0;
        }

        public async Task<long> DeleteAllUsersAsync()
        {
            var filter = new BsonDocument();
            var result = await _usersCollection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }
    }
}
