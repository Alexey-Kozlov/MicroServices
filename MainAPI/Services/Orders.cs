using Models;
using Services;
using MainAPI.Core;

namespace MainAPI.Services
{
    public class Orders : BaseService, IOrders
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Orders> _logger;
        public Orders(IHttpClientFactory clientFactory, IConfiguration config, ILogger<Orders> logger) 
            : base(clientFactory, logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<T> GetOrdersList<T>(string token, OrdersPageParams pageParams)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["ORDER_API"]!,
                Data = pageParams,
                Token = token
            });
        }

        public async Task<T> GetOrderById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["ORDER_API"]! + "/" + id.ToString(),
                Token = token
            });
        }

        public async Task<T> AddUpdateOrder<T>(OrderDTO order, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["ORDER_API"]!,
                Data = order,
                Token = token
            });
        }

        public async Task<T> DeleteOrder<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Delete,
                Url = _config["ORDER_API"]! + "/" + id.ToString(),
                Token = token
            });
        }
    }
}
