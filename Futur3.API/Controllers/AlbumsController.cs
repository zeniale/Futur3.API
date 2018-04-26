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
        private readonly AlbumPreviewService _albumPreviewService;

        public AlbumsController(AlbumPreviewService albumPreviewService)
        {
            this._albumPreviewService = albumPreviewService;
        }

        [HttpGet("preview")]
        public async Task<List<AlbumPreviewDto>> AlbumPreview()
        {
            var returnList= await this._albumPreviewService.GetAlbumsPreviewAsync();
            return returnList;
        }
    }
}