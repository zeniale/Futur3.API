using Microsoft.Extensions.Options;

using Futur3.Models;
using Futur3.Models.MongoDb;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

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
    }
}
