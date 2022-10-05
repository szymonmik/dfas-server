using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Services;

namespace server.Controllers;

[Route("api/product/")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    // GET ALL
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _productService.GetAll();
        
        return Ok(products);
    }
    
    // GET BY ID
    [HttpGet("{id}")]
    public ActionResult<Product> GetById([FromRoute]int id)
    {
        var product = _productService.GetById(id);

        return Ok(product);
    }
}