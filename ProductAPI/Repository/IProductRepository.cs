using ProductAPI.Models;

namespace ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int productId);
        Task<ProductDTO> CreateUpdateProduct(ProductDTO product);
        Task<bool> DeleteProduct(int productId);
    }
}
