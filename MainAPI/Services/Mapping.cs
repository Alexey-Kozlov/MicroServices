using AutoMapper;
using MIdentity;
using System.IdentityModel.Tokens.Jwt;

namespace MainAPI.Services
{
    public class Mapping
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<JwtSecurityToken, IdentityModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                    src.Claims.Where(p => p.Type == "unique_name").Select(p => p.Value).FirstOrDefault()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                    src.Claims.Where(p => p.Type == "nameid").Select(p => p.Value).FirstOrDefault()))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src =>
                    src.Claims.Where(p => p.Type == "given_name").Select(p => p.Value).FirstOrDefault()))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.Claims.Where(p => p.Type == "role").Select(p => p.Value).ToList<string>()));
            });
            return mappingConfig;
        }
    }
}
