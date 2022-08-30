using GlobalExceptionHandlingDemo.Dao;
using GlobalExceptionHandlingDemo.Data;
using GlobalExceptionHandlingDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace GlobalExceptionHandlingDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly DbContextClass _dbContext;

        public ProductService(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductDao>> GetProductList()
        {
            if (_dbContext.Products == null)
            {
                return Task.FromResult(new List<ProductDao>()).Result;
            }
            var result = await _dbContext.Products.ToListAsync();
            return result.Select(o => new ProductDao
            {
                ProductId = o.ProductId,
                ProductName = o.ProductName,
                ProductPrice = o.ProductPrice,
                ProductDescription = o.ProductDescription,
                ProductStocks = o.ProductStocks
            });

        }

        public async Task<ProductDao?> GetProductById(int id)
        {
            var result = await _dbContext.Products!.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }

            return new ProductDao
            {
                ProductId = result.ProductId,
                ProductName = result.ProductName,
                ProductPrice = result.ProductPrice,
                ProductDescription = result.ProductDescription,
                ProductStocks = result.ProductStocks
            };
        }

        public async Task<ProductDao> AddProduct(ProductDao product)
        {
            var result = _dbContext.Products!.Add(new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductStocks = product.ProductStocks
            });
            await _dbContext.SaveChangesAsync();
            var ret = result.Entity;
            return new ProductDao
            {
                ProductId = ret.ProductId,
                ProductName = ret.ProductName,
                ProductPrice = ret.ProductPrice,
                ProductDescription = ret.ProductDescription,
                ProductStocks = ret.ProductStocks
            };
        }

        public async Task<ProductDao> UpdateProduct(ProductDao product)
        {
            var result = _dbContext.Products!.Update(new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductStocks = product.ProductStocks
            }); 
            await _dbContext.SaveChangesAsync();
            var ret = result.Entity;
            return new ProductDao
            {
                ProductId = ret.ProductId,
                ProductName = ret.ProductName,
                ProductPrice = ret.ProductPrice,
                ProductDescription = ret.ProductDescription,
                ProductStocks = ret.ProductStocks
            };
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var filteredData = _dbContext.Products!.FirstOrDefault(x => x.ProductId == id);
            if(filteredData == null)
            {
                return false;
            }
            _dbContext.Remove(filteredData);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}