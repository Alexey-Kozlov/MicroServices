using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain;
using Models;
using ProductAPI.Persistance;

namespace ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ProductRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateUpdateProduct(ProductDTO productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                if (productDto.Id == 0)
                {
                    _appDbContext.Product.Add(product);
                }
                else
                {
                    _appDbContext.Product.Update(product);
                }
                await _appDbContext.SaveChangesAsync();
                return _mapper.Map<ProductDTO>(product);
            }
            catch(Exception e)
            {
                throw new Exception("Ошибка добавления/обновления product", e);
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _appDbContext.Product.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null) return false;
                _appDbContext.Product.Remove(product!);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Ошибка удаления product", e);
            }
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await _appDbContext.Product
                .Where(p => p.Id == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = await _appDbContext.Product.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }
    }
}
