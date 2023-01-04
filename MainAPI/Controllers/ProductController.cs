using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace MainAPI.Controllers
{
    [Authorize]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProducts _products;
        public ProductController(IProducts products)
        {
            _products = products;
        }

        [HttpGet("GetProductList")]
        public async Task<IActionResult> GetProductList()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _products.GetProductList(accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<List<ProductDTOFull>>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _products.GetProductById<ResponseDTO>(id, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddEditProduct([FromBody] ProductDTO product)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _products.AddUpdateProduct<ResponseDTO>(product, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _products.DeleteProduct<ResponseDTO>(id, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }
    }
}
