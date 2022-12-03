using AutoMapper;
using ProductAPI.Domain;
using ProductAPI.Models;

namespace ProductAPI.Core
{
    public class Mapping
    { 
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDTO>();
                config.CreateMap<ProductDTO, Product>();
            });
            return mappingConfig;
        }
    }
}
