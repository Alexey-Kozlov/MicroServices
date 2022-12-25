using MainAPI.Core;

namespace MainAPI.Services
{
    public interface IOrders
    {
        Task<T> GetOrdersList<T>(string token, OrdersPageParams pageParams);
        Task<T> GetOrderById<T>(int id, string token);
    }
}
