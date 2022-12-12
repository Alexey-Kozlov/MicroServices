using Services;
using Models;

namespace MainAPI.Services
{
    public class Products : BaseService, IProducts
    {
        private readonly IConfiguration _config;
        public Products(IHttpClientFactory clientFactory, IConfiguration config) : base(clientFactory)
        {
            _config = config;
        }
        public async Task<T> GetProductById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["ProductAPI"]! + "/api/products/" + id.ToString(),
                Token = token
            });
        }

        public async Task<T> GetProductList<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["ProductAPI"]! + "/api/products",
                Token = token
            });
        }

        public async Task<T> AddUpdateProduct<T>(ProductDTO product, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["ProductAPI"]! + "/api/products",
                Data = product,
                Token = token
            });
        }

        public async Task<T> DeleteProduct<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Delete,
                Url = _config["ProductAPI"]! + "/api/products/" + id.ToString(),
                Token = token
            });
        }
    }
}
