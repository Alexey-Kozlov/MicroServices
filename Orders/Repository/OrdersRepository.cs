using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Models;
using OrdersAPI.Core;
using OrdersAPI.Persistance;

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
                .Include(p => p.ProductIdList)
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
            var order = await _context.Order
                .Where(p => p.Id == orderId).FirstOrDefaultAsync();
            return _mapper.Map<OrderDTO>(order);
        }
    }
}
