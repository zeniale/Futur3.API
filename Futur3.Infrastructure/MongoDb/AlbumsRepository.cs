using Microsoft.Extensions.Options;

using Futur3.Models.MongoDb;
using Futur3.Models;

namespace Futur3.Infrastructure.MongoDb
{
    public class AlbumsRepository : GenericRepository<Album>
    {
        public AlbumsRepository(IOptions<ApplicationSettings> applicationSettings) : base(applicationSettings, applicationSettings.Value.MongoDbSettings.AlbumCollection, applicationSettings.Value.RemoteDataServiceSettings.AlbumsUrl) { }
    }
}
