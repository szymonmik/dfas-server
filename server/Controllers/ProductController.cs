using Microsoft.AspNetCore.Mvc;
using server.Entities;

namespace server.Controllers;

[Route("api/product/")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET ALL
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _dbContext.Products.ToList();
        
        return Ok(products);
    }
    
    // GET BY ID
    [HttpGet("{id}")]
    public ActionResult<Product> GetById([FromRoute]int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            NotFound();
        }
        
        return Ok(product);
    }
}