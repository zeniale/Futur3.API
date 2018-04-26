using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;

using Futur3.Models;
using Futur3.Models.DTO;
using Futur3.Models.MongoDb;
using Futur3.Infrastructure.MongoDb;

namespace Futur3.Infrastructure.Services
{
    public class AlbumsService
    {
        private readonly IMapper _mapper;
        private readonly UsersRepository _usersRepository;
        private readonly AlbumsRepository _albumsRepository;
        private readonly PhotosRepository _photosRepository;
        private readonly IOptions<ApplicationSettings> _applicationSettings;

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        protected readonly IMongoCollection<AlbumPreview> _albumPreviewCollection;

        public AlbumsService(
            IMapper mapper,
            UsersRepository usersRepository,
            AlbumsRepository albumsRepository,
            PhotosRepository photosRepository,
            IOptions<ApplicationSettings> applicationSettings
            )
        {
            this._mapper = mapper;
            this._usersRepository = usersRepository;
            this._albumsRepository = albumsRepository;
            this._photosRepository = photosRepository;
            this._applicationSettings = applicationSettings;

            this._client = new MongoClient(this._applicationSettings.Value.MongoDbSettings.ConnectionString);
            this._database = this._client.GetDatabase(this._applicationSettings.Value.MongoDbSettings.DatabaseName);
            this._albumPreviewCollection = this._database.GetCollection<AlbumPreview>(this._applicationSettings.Value.MongoDbSettings.AlbumPreviewCollection);
        }

        public async Task<List<AlbumPreviewDto>> GetAlbumsPreviewAsync()
        {
            List<AlbumPreview> albumsPreview = await this._albumPreviewCollection.Find(new BsonDocument()).ToListAsync();
            if (albumsPreview.Count == 0)
            {
                DateTime createdAt = DateTime.UtcNow;
                Random rnd = new Random();
                List<AlbumPreview> returnCollection = new List<AlbumPreview>();
                List<Album> albums = await this._albumsRepository.GetListAsync();
                List<User> users = await this._usersRepository.GetListAsync();

                foreach (Album album in albums)
                {
                    List<Photo> photos = await this._photosRepository.GetByAlbumIdAsync(album.ExternalId);
                    AlbumPreview albumPreview = this._mapper.Map<Album, AlbumPreview>(album);
                    this._mapper.Map<User, AlbumPreview>(users.SingleOrDefault(u => u.ExternalId == album.UserId), albumPreview);
                    albumPreview.PhotoCount = photos.Count;
                    albumPreview.RandomThumbnailUrl = photos.ElementAt(rnd.Next(photos.Count - 1)).ThumbnailUrl;
                    albumPreview.CreatedAt = createdAt;
                    returnCollection.Add(albumPreview);
                }
                albumsPreview = returnCollection;
                await this._albumPreviewCollection.InsertManyAsync(albumsPreview);
            }
            return this._mapper.Map<List<AlbumPreview>, List<AlbumPreviewDto>>(albumsPreview);
        }

        public async Task<long> DeleteAllAsync()
        {
            var filter = new BsonDocument();
            var result = await _albumPreviewCollection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

    }
}
