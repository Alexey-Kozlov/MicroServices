using Services;
using Models;

namespace MainAPI.Services
{
    public class Category : BaseService, ICategory
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Category> _logger;
        public Category(IHttpClientFactory clientFactory, IConfiguration config, ILogger<Category> logger) 
            : base(clientFactory, logger) 
        {
            _config= config;
            _logger= logger;
        }
        public async Task<T> GetCategoryById<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["CATEGORY_API"]! + "/" + id.ToString(), 
                Token = token
            });
        }

        public async Task<T> GetCategoryList<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["CATEGORY_API"]!,
                Token = token
            });
        }

        public async Task<T> AddUpdateCategory<T>(CategoryDTO category, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Post,
                Url = _config["CATEGORY_API"]!,
                Data= category,
                Token = token
            });
        }

        public async Task<T> DeleteCategory<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.Delete,
                Url = _config["CATEGORY_API"]! + "/" + id.ToString(),
                Token = token
            });
        }
    }
}
