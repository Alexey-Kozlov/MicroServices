using Models;
using OrdersAPI.Core;

namespace OrdersAPI.Repository
{
    public interface IOrdersRepository
    {
        Task<PagedList<OrderDTO>> List(OrdersPageParams pagingParams);
        Task<OrderDTO> GetOrderById(int orderId);
    }
}
