using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Futur3.Infrastructure.MongoDb;
using Futur3.Models.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace Futur3.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly AlbumsRepository _albumsRepository;

        public ValuesController(AlbumsRepository albumsRepository)
        {
            this._albumsRepository = albumsRepository;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("albums")]
        public async Task GetAlbums()
        {
            var aaa = await this._albumsRepository.GetAlbumsListAsync();
        }

        [HttpGet("insert-album")]
        public async Task InsertAlbum()
        {
            Album album = new Album()
            {
                Title = "Mio primo album",
                UserId = 1
            };
            await this._albumsRepository.InsertAlbumAsync(album);
        }

        [HttpGet("delete-albums")]
        public async Task DeleteAlbums()
        {
            var aaa = await this._albumsRepository.DeleteAllAlbumsAsync();
        }
    }
}
