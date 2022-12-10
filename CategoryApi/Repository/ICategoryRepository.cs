using Models;

namespace CategoryAPI.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetCategory();
        Task<CategoryDTO> GetCategoryById(int categoryId);
        Task<CategoryDTO> CreateUpdateCategory(CategoryDTO category);
        Task<bool> DeleteCategory(int categoryId);
    }
}
