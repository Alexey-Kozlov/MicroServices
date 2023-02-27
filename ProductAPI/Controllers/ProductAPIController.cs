using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers
{
    [Route("/")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this._response = new ResponseDTO();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                _response.Result = await _productRepository.GetProducts();

            }
            catch(Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDTO> Get(int id)
        {
            try
            {
                _response.Result = await _productRepository.GetProductById(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                _response.Result = await _productRepository.CreateUpdateProduct(productDTO);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ResponseDTO> Delete(int id)
        {
            try
            {
                _response.IsSuccess = await _productRepository.DeleteProduct(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}
