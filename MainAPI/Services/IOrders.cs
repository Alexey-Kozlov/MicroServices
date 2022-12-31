using MainAPI.Core;
using Models;

namespace MainAPI.Services
{
    public interface IOrders
    {
        Task<T> GetOrdersList<T>(string token, OrdersPageParams pageParams);
        Task<T> GetOrderById<T>(int id, string token);
        Task<T> AddUpdateOrder<T>(OrderDTO order, string token);
        Task<T> DeleteOrder<T>(int id, string token);
    }
}
