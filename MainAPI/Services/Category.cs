using Services;
using Models;

namespace MainAPI.Services
{
    public class Category : BaseService, ICategory
    {
        private readonly IConfiguration _config;
        public Category(IHttpClientFactory clientFactory, IConfiguration config): base(clientFactory) 
        {
            _config= config;
        }
        public async Task<T> GetCategoryById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Data= id,
                Url= _config["CategoryAPI"]! + "/api/category",
                Token = token
            });
        }

        public async Task<T> GetCategoryList<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Data = "",
                Url = _config["CategoryAPI"]! + "/api/category",
                Token = token
            });
        }
    }
}
