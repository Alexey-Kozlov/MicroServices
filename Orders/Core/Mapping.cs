using AutoMapper;
using OrdersAPI.Domain;
using Models;

namespace OrdersAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductIdList.Select(p => p.ProductId)));
        }
    }
}
