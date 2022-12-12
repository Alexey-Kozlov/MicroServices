using AutoMapper;
using ProductAPI.Domain;
using Models;

namespace ProductAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
