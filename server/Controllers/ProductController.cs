using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Models;
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
    [Authorize]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var products = _productService.GetAll(userId);
        
        return Ok(products);
    }
    
    // GET BY ID
    [HttpGet("{productId}")]
    [Authorize]
    public ActionResult<Product> GetById([FromRoute]int productId)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var product = _productService.GetById(userId, productId, User);

        return Ok(product);
    }
    
    // CREATE
    [HttpPost("create")]
    [Authorize]
    public ActionResult Create([FromBody] CreateProductDto dto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var id = _productService.CreateProduct(userId, dto);

        return Created($"/api/product/{id}", null);
    }
    
    // ADD ALLERGEN TO PRODUCT
    [HttpPost("{productId}/assignallergen/{allergenId}")]
    [Authorize]
    public ActionResult AssignAllergen([FromRoute] int productId, [FromRoute] int allergenId)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        _productService.AssignAllergen(productId, allergenId, User);

        return Ok();
    }
}