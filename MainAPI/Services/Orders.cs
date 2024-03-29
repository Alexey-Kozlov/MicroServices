﻿using Models;
using Services;
using MainAPI.Core;

namespace MainAPI.Services
{
    public class Orders : BaseService, IOrders
    {
        private readonly IConfiguration _config;
        public Orders(IHttpClientFactory clientFactory, IConfiguration config) : base(clientFactory)
        {
            _config = config;
        }

        public async Task<T> GetOrdersList<T>(string token, OrdersPageParams pageParams)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["OrdersAPI"]! + "/api/orders",
                Data = pageParams,
                Token = token
            });
        }

        public async Task<T> GetOrderById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["OrdersAPI"]! + "/api/orders/" + id.ToString(),
                Token = token
            });
        }

        public async Task<T> AddUpdateOrder<T>(OrderDTO order, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["OrdersAPI"]! + "/api/orders",
                Data = order,
                Token = token
            });
        }

        public async Task<T> DeleteOrder<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Delete,
                Url = _config["OrdersAPI"]! + "/api/orders/" + id.ToString(),
                Token = token
            });
        }
    }
}
