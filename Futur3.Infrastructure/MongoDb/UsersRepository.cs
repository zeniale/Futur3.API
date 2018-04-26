using Microsoft.Extensions.Options;

using Futur3.Models;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.MongoDb
{
    public class UsersRepository : GenericRepository<User>
    {
        public UsersRepository(IOptions<ApplicationSettings> applicationSettings) : base(applicationSettings, applicationSettings.Value.MongoDbSettings.UserCollection, applicationSettings.Value.RemoteDataServiceSettings.UsersUrl) { }
    }
}
