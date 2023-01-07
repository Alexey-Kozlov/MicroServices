using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Models;
using OrdersAPI.Core;
using OrdersAPI.Persistance;
using OrdersAPI.Domain;

namespace OrdersAPI.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrdersRepository(AppDbContext context, IMapper mapper, ILogger<OrdersRepository> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<OrderDTO>> List(OrdersPageParams pagingParams)
        {
            var query = _context.Order
                .Include(p => p.Products)
                .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();
            if(!string.IsNullOrEmpty(pagingParams.UserId))
            {
                query = query.Where(p => p.UserId == pagingParams.UserId);
            }
            if(!string.IsNullOrEmpty(pagingParams.SortField))
            {
                if (pagingParams.SortDirection == "asc")
                {
                    if (pagingParams.SortField == "Products")
                    {
                        //в случае сортировки по количеству позиций в заказе
                        query = query.OrderBy(p => p.Products.Count());
                    }
                    else
                    {
                        query = query.OrderBy(p => EF.Property<object>(p, pagingParams.SortField));
                    }
                }
                else
                {
                    if (pagingParams.SortField == "Products")
                    {
                        //в случае сортировки по количеству позиций в заказе
                        query = query.OrderByDescending(p => p.Products.Count());
                    }
                    else
                    {
                        query = query.OrderByDescending(p => EF.Property<object>(p, pagingParams.SortField));
                    }
                }
            }
            return await PagedList<OrderDTO>.CreateAsync(query, pagingParams);
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var item = await _context.Order
                .Include(p => p.Products)
                .Where(p => p.Id == orderId)
                .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (item != null)
                return item;
            return new OrderDTO();
        }

        public async Task<OrderDTO> CreateUpdateOrder(OrderDTO orderDto)
        {
            try
            {
                if (orderDto.Id == 0)
                {
                    _context.Order.Add(_mapper.Map<Order>(orderDto));
                }
                else
                {
                    var oldOrder = await _context.Order
                        .Include(p => p.Products)
                        .Where(p => p.Id == orderDto.Id)
                        .FirstOrDefaultAsync();
                    if (oldOrder != null)
                    {
                        oldOrder.OrderDate = orderDto.OrderDate;
                        oldOrder.UserId= orderDto.UserId;
                        oldOrder.Description= orderDto.Description;
                        var existingProdId = new List<int>();
                        var deletingProdId = new List<int>();
                        foreach(var prod in orderDto.Products)
                        {
                            if(prod.Id < 0)
                            {
                                //это новая запрись
                                oldOrder.Products.Add(new ProductRef
                                {
                                    ProductId = prod.ProductId,
                                    OrderId = orderDto.Id,
                                    Quantity = prod.Quantity
                                });
                            }
                            else
                            {
                                //это редактирование имеющейся
                                var _prod = oldOrder.Products.FirstOrDefault(p => p.Id == prod.Id);
                                if (_prod != null)
                                {
                                    _prod.ProductId = prod.ProductId;
                                    _prod.Quantity = prod.Quantity;
                                }
                                existingProdId.Add(prod.Id);
                            }
                        }
                        //проверяем на удаление записей продуктов
                        foreach(var item in oldOrder.Products)
                        {
                            if(!existingProdId.Any(p => p == item.Id) && item.Id != 0)
                            {
                                deletingProdId.Add(item.Id);
                            }
                        }
                        foreach(var deletedId in deletingProdId)
                        {
                            oldOrder.Products.Remove(oldOrder.Products.FirstOrDefault(p => p.Id == deletedId)!);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return orderDto;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка добавления/обновления заказа", e);
            }
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _context.Order.FirstOrDefaultAsync(p => p.Id == orderId);
                if (order == null) return false;
                _context.Order.Remove(order!);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка удаления заказа", e);
            }
        }
    }
}
