using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace MainAPI.Controllers
{
    [Authorize]
    [Route("ms/api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategory category, ILogger<CategoryController> logger)
        {
            _category = category;
            _logger = logger;   
        }
        [HttpGet("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _category.GetCategoryList<ResponseDTO>(accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _category.GetCategoryById<ResponseDTO>(id, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]CategoryDTO category)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _category.AddUpdateCategory<ResponseDTO>(category, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _category.DeleteCategory<ResponseDTO>(id, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }
    }
}
