using Microsoft.AspNetCore.Mvc;
using Models;
using CategoryAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CategoryAPI.Controllers
{
    [Authorize]
    [Route("api/category")]
    public class CategoryAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryAPIController(ICategoryRepository categoryRepository)
        {
            this._response = new ResponseDTO();
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                _response.Result = await _categoryRepository.GetCategoryList();

            }
            catch (Exception e)
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
                _response.Result = await _categoryRepository.GetCategoryById(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                _response.Result = await _categoryRepository.CreateUpdateCategory(categoryDTO);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ResponseDTO> Delete(int id)
        {
            try
            {
                _response.IsSuccess = await _categoryRepository.DeleteCategory(id);

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
