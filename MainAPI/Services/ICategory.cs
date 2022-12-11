namespace MainAPI.Services
{
    public interface ICategory
    {
        Task<T> GetCategoryList<T>(string token);
        Task<T> GetCategoryById<T>(int id, string token);
    }
}
