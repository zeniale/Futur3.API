using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using Futur3.Models.DTO;
using Futur3.Models.MongoDb;
using Futur3.Infrastructure.MongoDb;

namespace Futur3.Infrastructure.Services
{
    public class PhotosService
    {
        private readonly IMapper _mapper;
        private readonly PhotosRepository _photosRepository;

        public PhotosService(
            IMapper mapper,
            PhotosRepository photosRepository
            )
        {
            this._mapper = mapper;
            this._photosRepository = photosRepository;
        }

        public async Task<List<PhotoPreviewDto>> GetByAlbumIdAsync(int albumId)
        {
            return this._mapper.Map<List<Photo>, List<PhotoPreviewDto>>(await this._photosRepository.GetByAlbumIdAsync(albumId));
        }

        public async Task<PhotoDto> GetByIdAsync(int photoId)
        {
            return this._mapper.Map<Photo, PhotoDto>(await this._photosRepository.GetByExternalIdAsync(photoId));
        }

        public async Task<bool> IncreaseLikes(int photoId)
        {
            return await this._photosRepository.IncreaseLikes(photoId);
        }
    }
}
