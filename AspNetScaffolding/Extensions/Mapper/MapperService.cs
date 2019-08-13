using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetScaffolding.Extensions.Mapper
{
    public static class MapperService
    {
        public static void SetupAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configureMapper)
        {
            if (configureMapper == null)
            {
                return;
            }

            var mapperConfiguration= new MapperConfiguration(config =>
            {
                configureMapper.Invoke(config);
            });

            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton(mapper);

            GlobalMapper.Mapper = mapper;
        }
    }
}
