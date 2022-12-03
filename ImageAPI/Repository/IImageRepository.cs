using ImageAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Repository
{
    public interface IImageRepository
    {
        Task<List<ImageDTO>> GetImages();
        Task<ImageDTO> GetImageById(int id);
        Task<bool> Delete(int id);
        Task<ImageDTO> CreateUpdateImage([FromBody] ImageDTO imageDto);
    }
}
