using Services;
using Models;
using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using MainAPI.Core;

namespace MainAPI.Services
{
    public class Products : BaseService, IProducts
    {
        private readonly IConfiguration _config;
        private readonly ICategory _category;
        private readonly IMapper _mapper;
        public Products(IHttpClientFactory clientFactory, IConfiguration config, 
            ICategory category, IMapper mapper) : base(clientFactory)
        {
            _config = config;
            _category = category;
            _mapper = mapper;
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
        public async Task<ResponseDTO> GetProductList(string token)
        {
            var productParam = new ApiRequest()
            {
                ApiType = ApiType.Get,
                Url = _config["ProductAPI"]! + "/api/products",
                Token = token
            };
            //параллельно запускаем оба потока на получение данных
            var productList = Task.Run(() => SendAsync<ResponseDTO>(productParam));
            var categoryList = Task.Run(() => _category.GetCategoryList<ResponseDTO>(token));
            //ждем выполнения обоих запросов
            await Task.WhenAll(productList, categoryList);
            if(productList.Result != null && productList.Result.IsSuccess)
            {
                var productDTO = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(productList.Result.Result)!);
                if (categoryList.Result != null && categoryList.Result.IsSuccess)
                {
                    var categoryDTO = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryList.Result.Result)!);
                    var result = productDTO!.Join(categoryDTO!, p => p.CategoryId, q => q.Id, (p, q) =>
                    new ProductDTOFull
                    {
                        Id = p.Id,
                        CategoryId = p.CategoryId,
                        CategoryName = q.Name,
                        Description = p.Description,
                        ImageId = p.ImageId,
                        Name = p.Name,
                        Price = p.Price
                    });
                    return new ResponseDTO { IsSuccess = true, 
                        Result = JsonConvert.SerializeObject(result) };
                }
                return new ResponseDTO { IsSuccess = true, 
                    Result = JsonConvert.SerializeObject(_mapper.Map<List<ProductDTOFull>>(productDTO!)) };
            }
            return new ResponseDTO();
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
