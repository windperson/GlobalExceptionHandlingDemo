using GlobalExceptionHandlingDemo.Dao;

namespace GlobalExceptionHandlingDemo.Services;

public interface IProductService
{
    public Task<IEnumerable<ProductDao>> GetProductList();
    public Task<ProductDao?> GetProductById(int id);
    public Task<ProductDao> AddProduct(ProductDao product);
    public Task<ProductDao> UpdateProduct(ProductDao product);
    public Task<bool> DeleteProduct(int id);
}