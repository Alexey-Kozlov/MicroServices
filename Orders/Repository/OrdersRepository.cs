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
            return await PagedList<OrderDTO>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            return await _context.Order
                .Include(p => p.Products)
                .Where(p => p.Id == orderId)
                .ProjectTo<OrderDTO>(_mapper.ConfigurationProvider)
                .DefaultIfEmpty(new OrderDTO())
                .FirstAsync();
        }

        public async Task<OrderDTO> CreateUpdateOrder(OrderDTO orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                if (orderDto.Id == 0)
                {
                    _context.Order.Add(order);
                }
                else
                {
                    _context.Order.Update(order);
                }
                await _context.SaveChangesAsync();
                return _mapper.Map<OrderDTO>(order);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка добавления/обновления заказа", e);
            }
        }
    }
}
