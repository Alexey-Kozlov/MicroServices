using AutoMapper;
using CategoryAPI.Domain;
using CategoryAPI.Models;

namespace CategoryAPI.Core
{
    public class Mapping
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Category, CategoryDTO>();
                config.CreateMap<CategoryDTO, Category>();
            });
            return mappingConfig;
        }
    }
}
