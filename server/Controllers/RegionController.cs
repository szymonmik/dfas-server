using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/region/")]
public class RegionController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    /// <summary>
    /// Gets all regions
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<Region>> GetAll()
    {
        var regions = _regionService.GetAll();

        return Ok(regions);
    }
    
    /// <summary>
    /// Gets region by id
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Region> GetById([FromRoute]int id)
    {
        var region = _regionService.GetById(id);

        return Ok(region);
    }
}