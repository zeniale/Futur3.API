using Futur3.Infrastructure.MongoDb;
using Futur3.Models.DTO;
using Futur3.Models.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futur3.Infrastructure.Services
{
    public class AlbumPreviewService
    {
        private readonly AlbumsRepository _albumsRepository;
        private readonly UsersRepository _usersRepository;

        public AlbumPreviewService(
            AlbumsRepository albumsRepository,
            UsersRepository usersRepository
            )
        {
            this._albumsRepository = albumsRepository;
            this._usersRepository = usersRepository;
        }

        public async Task<List<AlbumPreview>> GetAlbumsPreviewAsync()
        {
            List<AlbumPreview> returnCollection = new List<AlbumPreview>();
            List<Album> albums = await this._albumsRepository.GetAlbumsListAsync();
            List<User> users = await this._usersRepository.GetAllUsersListAsync();

            foreach (Album album in albums)
            {
                returnCollection.Add(
                    new AlbumPreview
                    {
                        Album = album,
                        User = users.SingleOrDefault(u => u.ExternalId == album.UserId)
                    }
                );
            }
            return returnCollection;
        }
    }
}
