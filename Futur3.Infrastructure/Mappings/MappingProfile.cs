using AutoMapper;

using Futur3.Models.DTO;
using Futur3.Models.MongoDb;

namespace Futur3.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Album, AlbumPreview>()
                .ForMember(dest => dest.AlbumId, opt => opt.MapFrom(s => s.ExternalId))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<User, AlbumPreview>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(dest => dest.WebSite, opt => opt.MapFrom(s => s.Website))
                .ForMember(dest => dest.City, opt => opt.MapFrom(s => s.Address.City))
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(s => s.Address.Geo.Lat))
                .ForMember(dest => dest.Lng, opt => opt.MapFrom(s => s.Address.Geo.Lng))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
