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
                Url = _config["CategoryAPI"]! + "/api/category/" + id.ToString(), 
                Token = token
            });
        }

        public async Task<T> GetCategoryList<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["CategoryAPI"]! + "/api/category",
                Token = token
            });
        }

        public async Task<T> AddUpdateCategory<T>(CategoryDTO category, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["CategoryAPI"]! + "/api/category",
                Data= category,
                Token = token
            });
        }

        public async Task<T> DeleteCategory<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Delete,
                Url = _config["CategoryAPI"]! + "/api/category/" + id.ToString(),
                Token = token
            });
        }
    }
}
