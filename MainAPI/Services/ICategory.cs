using Models;

namespace MainAPI.Services
{
    public interface ICategory
    {
        Task<T> GetCategoryList<T>(string token);
        Task<T> GetCategoryById<T>(int id, string token);
        Task<T> AddUpdateCategory<T>(CategoryDTO category, string token);
        Task<T> DeleteCategory<T>(int id, string token);
    }
}
