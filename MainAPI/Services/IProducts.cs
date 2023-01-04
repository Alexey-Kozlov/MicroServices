using Models;
namespace MainAPI.Services
{
    public interface IProducts
    {
        Task<T> GetProductById<T>(int id, string token);
        Task<ResponseDTO> GetProductList(string token);
        Task<T> AddUpdateProduct<T>(ProductDTO product, string token);
        Task<T> DeleteProduct<T>(int id, string token);
    }
}
