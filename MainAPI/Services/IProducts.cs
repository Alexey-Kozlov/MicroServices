namespace MainAPI.Services
{
    public interface IProducts
    {
        Task<T> GetProducts<T>(string token);
        Task<T> GetProductById<T>(int id, string token);
    }
}
