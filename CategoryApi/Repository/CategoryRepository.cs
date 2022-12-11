using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CategoryAPI.Domain;
using Models;
using CategoryAPI.Persistance;

namespace CategoryAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateUpdateCategory(CategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                if (categoryDto.Id == 0)
                {
                    _appDbContext.Category.Add(category);
                }
                else
                {
                    _appDbContext.Category.Update(category);
                }
                await _appDbContext.SaveChangesAsync();
                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка добавления/обновления категории", e);
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _appDbContext.Category.FirstOrDefaultAsync(p => p.Id == categoryId);
                if (category == null) return false;
                _appDbContext.Category.Remove(category!);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка удаления категории", e);
            }
        }

        public async Task<CategoryDTO> GetCategoryById(int categoryId)
        {
            var category = await _appDbContext.Category
                .Where(p => p.Id == categoryId).FirstOrDefaultAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoryList()
        {
            var category = await _appDbContext.Category.ToListAsync();
            return _mapper.Map<List<CategoryDTO>>(category);
        }
    }
}
