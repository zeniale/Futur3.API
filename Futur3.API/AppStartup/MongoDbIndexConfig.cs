using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

using Futur3.Models;
using Futur3.Infrastructure.MongoDb;

namespace Futur3.API.AppStartup
{
    public static class MongoDbIndexConfig
    {
        public static void ConfigureMongoDbIndexes(this IServiceCollection services)
        {
            var appSettings = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationSettings>>().Value;
            IndexesGenerator.CreateIndexes(appSettings);
        }

    }
}
