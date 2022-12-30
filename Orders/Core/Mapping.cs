using AutoMapper;
using OrdersAPI.Domain;
using OrdersAPI.Models;

namespace OrdersAPI.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => 
                src.Products.Select(p => new ProductItemsDTO { Id = p.Id, ProductId = p.ProductId , Quantity = p.Quantity })));
            CreateMap<OrderDTO, Order>()

        }
    }
}
