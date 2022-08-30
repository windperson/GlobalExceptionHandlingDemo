using GlobalExceptionHandlingDemo.Dao;
using GlobalExceptionHandlingDemo.Exceptions;
using GlobalExceptionHandlingDemo.Model;
using GlobalExceptionHandlingDemo.Services;
using Microsoft.AspNetCore.Mvc;
using NotImplementedException = GlobalExceptionHandlingDemo.Exceptions.NotImplementedException;

namespace GlobalExceptionHandlingDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private ILogger<ProductController> _logger;
    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    
    [HttpGet("productlist")]
    public Task<IEnumerable<ProductDao>> ProductList()
    {
        return _productService.GetProductList();
    }
    
    [HttpGet("getproductbyid")]
    public async Task<ProductDao?> GetProductById(int id)
    {
        _logger.LogInformation("Fetch Product with ID: {Id} from the database", id);
        var product = await _productService.GetProductById(id);
        if (product == null)
        {
            throw new NotFoundException($"Product ID {id} not found.");
        }
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation("Returning product with ID: {ProductId}", product.ProductId);
        return product;
    }
    
    [HttpPost("addproduct")]
    public Task<ProductDao> AddProduct([FromBody] ProductDao product)
    {
        return _productService.AddProduct(product);
    }
    
    [HttpPut("updateproduct")]
    public Task<ProductDao> UpdateProduct([FromBody] ProductDao product)
    {
        return _productService.UpdateProduct(product);
    }
    
    [HttpDelete("deleteproduct")]
    public Task<bool> DeleteProduct(int id)
    {
        return _productService.DeleteProduct(id);
    }

    [HttpGet("filterproduct")]
    public Task<List<Product>> FilterProduct(int Id)
    {
        throw new NotImplementedException("Not Implemented!");
    }
}