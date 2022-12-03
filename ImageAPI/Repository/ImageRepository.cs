using AutoMapper;
using ImageAPI.Domain;
using ImageAPI.Models;
using ImageAPI.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace ImageAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public ImageRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;   
            _mapper = mapper;
        }
        public async Task<List<ImageDTO>> GetImages()
        {
            var images = await _appDbContext.Image.ToListAsync();
            return _mapper.Map<List<ImageDTO>>(images);
        }

        public async Task<ImageDTO> GetImageById(int id)
        {
            var image = await _appDbContext.Image.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ImageDTO>(image);
        }

        public async Task<bool> Delete(int id)
        {
            var image = _appDbContext.Image.FirstOrDefault(p => p.Id == id);
            if (image == null) return false;
            _appDbContext.Image.Remove(image);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ImageDTO> CreateUpdateImage([FromBody] ImageDTO imageDto)
        {
            try
            {
                var image = _mapper.Map<Image>(imageDto);
                if (imageDto.Id == 0)
                {
                    _appDbContext.Image.Add(image);
                }
                else
                {
                    _appDbContext.Image.Update(image);
                }
                await _appDbContext.SaveChangesAsync();
                return _mapper.Map<ImageDTO>(image);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка добавления/обновления image", e);
            }
        }
    }
}
