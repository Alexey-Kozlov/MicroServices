using AutoMapper;
using RabbitConsumer.Domain;
using RabbitConsumer.Models;
using RabbitConsumer.Services;

namespace RabbitConsumer
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LogMessageDTO, LogMessage>()
                .ForMember(dest => dest.TypeData, opt => opt.MapFrom(src => src.data))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.action));
        }
    }
}
