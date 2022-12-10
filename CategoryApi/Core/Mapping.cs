using AutoMapper;
using CategoryAPI.Domain;
using Models;

namespace CategoryAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
