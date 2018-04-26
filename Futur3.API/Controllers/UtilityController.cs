
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Futur3.Infrastructure.MongoDb;
using Futur3.Infrastructure.Services;

namespace Futur3.API.Controllers
{
    [Route("api/[controller]")]
    public class UtilityController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly AlbumsRepository _albumsRepository;
        private readonly PhotosRepository _photosRepository;
        private readonly AlbumPreviewService _albumPreviewService;

        public UtilityController(
            UsersRepository usersRepository,
            AlbumsRepository albumsRepository,
            PhotosRepository photosRepository,
            AlbumPreviewService albumPreviewService
            )
        {
            this._usersRepository = usersRepository;
            this._albumsRepository = albumsRepository;
            this._photosRepository = photosRepository;
            this._albumPreviewService = albumPreviewService;
        }

        [HttpGet("clean-mongo-data")]
        public async Task CleanMongoData()
        {
            await this._usersRepository.DeleteAllAsync();
            await this._albumsRepository.DeleteAllAsync();
            await this._photosRepository.DeleteAllAsync();
            await this._albumPreviewService.DeleteAllAsync();
        }
    }
}