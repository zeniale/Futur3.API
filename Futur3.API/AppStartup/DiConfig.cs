using Microsoft.Extensions.DependencyInjection;

using Futur3.Infrastructure.MongoDb;
using Futur3.Infrastructure.Services;

namespace Futur3.API.AppStartup
{
    public static class DiConfig
    {
        public static void AddDi(this IServiceCollection services)
        {
            services.AddScoped<AlbumsRepository>();
            services.AddScoped<UsersRepository>();

            services.AddScoped<AlbumPreviewService>();
        }
    }
}
