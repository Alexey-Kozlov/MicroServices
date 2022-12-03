using AutoMapper;
using ImageAPI.Domain;
using ImageAPI.Models;

namespace ImageAPI.Core
{
    public class Mapping
    { 
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Image, ImageDTO>()
                    .ForMember(dest => dest.Data,
                    opt => opt.MapFrom(src => Convert.ToBase64String(src.Data)));

                config.CreateMap<ImageDTO, Image>()
                    .ForMember(dest => dest.Data,
                    opt => opt.MapFrom(src => Convert.FromBase64String(src.Data)));
            });
            return mappingConfig;
        }
    }
}
