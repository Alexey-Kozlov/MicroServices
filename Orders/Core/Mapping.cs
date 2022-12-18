using AutoMapper;
using OrdersAPI.Domain;
using Models;

namespace OrdersAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
