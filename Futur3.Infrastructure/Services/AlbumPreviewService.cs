using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using Futur3.Models.DTO;
using Futur3.Models.MongoDb;
using Futur3.Infrastructure.MongoDb;

namespace Futur3.Infrastructure.Services
{
    public class AlbumPreviewService
    {
        private readonly IMapper _mapper;
        private readonly AlbumsRepository _albumsRepository;
        private readonly UsersRepository _usersRepository;

        public AlbumPreviewService(
            IMapper mapper,
            AlbumsRepository albumsRepository,
            UsersRepository usersRepository
            )
        {
            this._mapper = mapper;
            this._albumsRepository = albumsRepository;
            this._usersRepository = usersRepository;
        }

        public async Task<List<AlbumPreview>> GetAlbumsPreviewAsync()
        {
            List<AlbumPreview> returnCollection = new List<AlbumPreview>();
            List<Album> albums = await this._albumsRepository.GetListAsync();
            List<User> users = await this._usersRepository.GetListAsync();

            foreach (Album album in albums)
            {
                AlbumPreview albumPreview = this._mapper.Map<Album, AlbumPreview>(album);
                this._mapper.Map<User, AlbumPreview>(users.SingleOrDefault(u => u.ExternalId == album.UserId), albumPreview);
                returnCollection.Add(albumPreview);
            }
            return returnCollection;
        }
    }
}
