﻿using Microsoft.AspNetCore.Mvc;
using CategoryAPI.Models;
using CategoryAPI.Repository;

namespace CategoryAPI.Controllers
{
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
                _response.Result = await _categoryRepository.GetCategory();

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
