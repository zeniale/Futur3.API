using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Futur3.Models.DTO;
using Futur3.Infrastructure.Services;

namespace Futur3.API.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {
        private readonly AlbumsService _albumPreviewService;
        private readonly PhotosService _photosService;

        public AlbumsController(
            AlbumsService albumPreviewService,
            PhotosService photosService
            )
        {
            this._albumPreviewService = albumPreviewService;
            this._photosService = photosService;
        }

        [HttpGet("preview")]
        public async Task<List<AlbumPreviewDto>> AlbumPreview()
        {
            return await this._albumPreviewService.GetAlbumsPreviewAsync();
        }

        [HttpGet("preview/{albumId}")]
        public async Task<List<PhotoPreviewDto>> PhotosPreview(int albumId)
        {
            return await this._photosService.GetByAlbumIdAsync(albumId);
        }

        [HttpGet("photo/{photoId}")]
        public async Task<PhotoDto> GetPhotoById(int photoId)
        {
            return await this._photosService.GetByIdAsync(photoId);
        }

        [HttpPatch("increase-likes/{photoId}")]
        public async Task<bool> IncreaseLike(int photoId)
        {
            return await this._photosService.IncreaseLikes(photoId);
        }
    }
}