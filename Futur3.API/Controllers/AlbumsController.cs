using Futur3.Infrastructure.MongoDb;
using Futur3.Infrastructure.Services;
using Futur3.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Futur3.API.Controllers
{
    [Route("api/Albums")]
    public class AlbumsController : Controller
    {
        private readonly AlbumPreviewService _albumPreviewService;

        public AlbumsController(AlbumPreviewService albumPreviewService)
        {
            this._albumPreviewService = albumPreviewService;
        }

        [HttpGet("preview")]
        public async Task<List<AlbumPreview>> AlbumPreview()
        {
            var returnList= await this._albumPreviewService.GetAlbumsPreviewAsync();
            return returnList;
        }
    }
}