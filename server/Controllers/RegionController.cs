using Microsoft.AspNetCore.Mvc;
using server.Entities;

namespace server.Controllers;

[ApiController]
[Route("api/region/")]
public class RegionController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public RegionController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // GET ALL
    [HttpGet]
    public ActionResult<IEnumerable<Region>> GetAll()
    {
        var regions = _dbContext.Regions.ToList();

        return Ok(regions);
    }
    
    // GET BY ID
    [HttpGet("{id}")]
    public ActionResult<Region> GetById([FromRoute]int id)
    {
        var region = _dbContext.Regions.FirstOrDefault(v => v.Id == id);

        if (region is null)
        {
            return NotFound();
        }
        
        return Ok(region);
    }
}