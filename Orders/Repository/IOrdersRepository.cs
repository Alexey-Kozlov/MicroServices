using OrdersAPI.Core;
using OrdersAPI.Models;

namespace OrdersAPI.Repository
{
    public interface IOrdersRepository
    {
        Task<PagedList<OrderDTO>> List(OrdersPageParams pagingParams);
        Task<OrderDTO?> GetOrderById(int orderId);
    }
}
