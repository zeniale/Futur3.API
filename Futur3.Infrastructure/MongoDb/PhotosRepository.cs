using Microsoft.Extensions.Options;

using Futur3.Models;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public class PhotosRepository : GenericRepository<Photo>
    {
        public PhotosRepository(IOptions<ApplicationSettings> applicationSettings) : base(applicationSettings, applicationSettings.Value.MongoDbSettings.PhotoCollection, applicationSettings.Value.RemoteDataServiceSettings.PhotosUrl) { }
    }
}
