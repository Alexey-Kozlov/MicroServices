using ImageAPI.Models;
using ImageAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Controllers
{
    [Route("api/images")]
    public class ImageAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private readonly IImageRepository _imageRepository;
        public ImageAPIController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            this._response = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> Get()
        {
            try
            {
                _response.Result = await _imageRepository.GetImages();

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
                _response.Result = await _imageRepository.GetImageById(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] ImageDTO imageDTO)
        {
            try
            {
                _response.Result = await _imageRepository.CreateUpdateImage(imageDTO);

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
                _response.IsSuccess = await _imageRepository.Delete(id);

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
