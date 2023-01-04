using AutoMapper;
using MIdentity;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace MainAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductDTO, ProductDTOFull>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => ""));
        }
    }
}
