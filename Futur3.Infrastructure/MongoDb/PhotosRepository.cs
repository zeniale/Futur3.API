using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

using Futur3.Models;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public class PhotosRepository : GenericRepository<Photo>
    {
        public PhotosRepository(IOptions<ApplicationSettings> applicationSettings) : base(applicationSettings, applicationSettings.Value.MongoDbSettings.PhotoCollection, applicationSettings.Value.RemoteDataServiceSettings.PhotosUrl) { }

        public async Task<List<Photo>> GetByAlbumIdAsync(int albumId)
        {
            await this.EnsureCollectionLoaded();
            var filter = Builders<Photo>.Filter.Eq("albumId", albumId);
            var result = await _collection.Find(filter).ToListAsync();

            return result;
        }

        public async Task<bool> IncreaseLikes(int photoId)
        {
            UpdateDefinition<Photo> updateDef = Builders<Photo>.Update.Inc<int>(f => f.Likes, 1);

            UpdateResult updateResult = await _collection.UpdateOneAsync(o => o.ExternalId == photoId, updateDef);
            return updateResult.ModifiedCount > 0;
        }
    }
}
