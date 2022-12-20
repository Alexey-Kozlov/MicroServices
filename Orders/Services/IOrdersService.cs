using Models;
using OrdersAPI.Core;

namespace OrdersAPI.Services
{
    public interface IOrdersService
    {
        Task<PagedList<OrderDTO>> List(OrdersPageParams pagingParams);
    }
}
