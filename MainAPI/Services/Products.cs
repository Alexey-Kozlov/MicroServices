using Services;
using Models;

namespace MainAPI.Services
{
    public class Products : BaseService, IProducts
    {
        private readonly IConfiguration _config;
        public Products(IHttpClientFactory clientFactory, IConfiguration config): base(clientFactory) 
        {
            _config= config;
        }
        public async Task<T> GetProductById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Data= id,
                Url= _config["ProductAPI"]!,
                Token = token
            });
        }

        public async Task<T> GetProducts<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Data = "",
                Url = _config["ProductAPI"]!,
                Token = token
            });
        }
    }
}
