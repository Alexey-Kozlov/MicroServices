using AutoMapper;
using Identity.Models;

namespace Identity.Services
{
    public class Mapping
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName));
            });
            return mappingConfig;
        }
    }
}
