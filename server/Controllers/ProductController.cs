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
    
    /// <summary>
    /// Gets all global and own products
    /// </summary>
    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var products = _productService.GetAll(userId);
        
        return Ok(products);
    }
    
    /// <summary>
    /// Gets all own products
    /// </summary>
    [HttpGet("own")]
    [Authorize]
    public ActionResult<IEnumerable<Product>> GetAllCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var products = _productService.GetAllCurrentUser(userId);
        
        return Ok(products);
    }
    
    /// <summary>
    /// Gets global or own product by id
    /// </summary>
    [HttpGet("{productId}")]
    [Authorize]
    public ActionResult<Product> GetById([FromRoute]int productId)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var product = _productService.GetById(userId, productId, User);

        return Ok(product);
    }
    
    /// <summary>
    /// Creates product
    /// </summary>
    [HttpPost]
    [Authorize]
    public ActionResult Create([FromBody] CreateProductDto dto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var id = _productService.CreateProduct(userId, dto);

        return Created($"/api/product/{id}", null);
    }
    
    /// <summary>
    /// Updates product
    /// </summary>
    /// <remarks>Only name so far</remarks>
    [HttpPost("update/{productId}")]
    [Authorize]
    public ActionResult Update([FromRoute] int productId, [FromBody] UpdateProductDto dto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        _productService.UpdateProduct(userId, productId, dto, User);

        return Ok();
    }
    
    /// <summary>
    /// Deletes product and existing allergen assignments
    /// </summary>
    [HttpDelete("{productId}")]
    [Authorize]
    public ActionResult Delete(int productId)
    {
        _productService.DeleteProduct(productId, User);

        return Ok();
    }
    
    /// <summary>
    /// Assigns allergen to own product
    /// </summary>
    [HttpPost("{productId}/assignallergen/{allergenId}")]
    [Authorize]
    public ActionResult AssignAllergen([FromRoute] int productId, [FromRoute] int allergenId)
    {
        _productService.AssignAllergen(productId, allergenId, User);

        return Ok();
    }
    
    /// <summary>
    /// Unassigns allergen from own product
    /// </summary>
    /// <remarks>Only own products</remarks>
    /// <response code="204">Unassigned succesfully</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Product has missing/invalid values</response>
    /// <response code="500">Server error</response>
    [HttpPost("{productId}/unassignallergen/{allergenId}")]
    [Authorize]
    public ActionResult UnassignAllergen([FromRoute] int productId, [FromRoute] int allergenId)
    {
        _productService.UnassignAllergen(productId, allergenId, User);

        return Ok();
    }
    
    /// <summary>
    /// For testing purposes, do not use
    /// </summary>
    [HttpGet("filtered")]
    public ActionResult<Product> GetFiltered()
    {
        var products = _productService.GetFiltered();

        return Ok(products);
    }
}