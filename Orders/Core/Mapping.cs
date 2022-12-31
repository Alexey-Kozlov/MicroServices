using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
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
                .ForMember(dest => dest.Products, opt => opt.MapFrom((src, dest) =>
                {
                    foreach(var item in src.Products)
                    {
                        dest.Products.Add(new ProductRef
                        {
                            Id = item.Id <0 ? 0 : item.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            OrderId = src.Id
                        });
                    }
                    return dest.Products;
                }));

        }
    }
}
