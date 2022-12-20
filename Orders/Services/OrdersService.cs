using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Models;
using OrdersAPI.Core;
using OrdersAPI.Persistance;

namespace OrdersAPI.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrdersService(AppDbContext context, IMapper mapper, ILogger<OrdersService> logger)
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
    }
}
