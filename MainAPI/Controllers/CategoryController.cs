using MainAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace MainAPI.Controllers
{
    [Authorize]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        public CategoryController(ICategory category)
        {
            _category= category;
        }
        [HttpGet("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {

            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _category.GetCategoryList<ResponseDTO>(accessToken!);
            if(response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();

        }
    }
}
