using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;
using AutoMapper.EquivalencyExpression;

using Futur3.Infrastructure.Mappings;

namespace Futur3.API.AppStartup
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            Mapper.Reset();
            services.AddAutoMapper(cfg => cfg.AddCollectionMappers(), new Assembly[] { typeof(MappingProfile).GetTypeInfo().Assembly });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
