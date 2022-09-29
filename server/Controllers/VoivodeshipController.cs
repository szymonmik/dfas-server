using Microsoft.AspNetCore.Mvc;
using server.Entities;

namespace server.Controllers;

[ApiController]
[Route("api/voivodeship/")]
public class VoivodeshipController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public VoivodeshipController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // GET ALL
    [HttpGet]
    public ActionResult<IEnumerable<Voivodeship>> GetAll()
    {
        var voivodeships = _dbContext.Voivodeships.ToList();

        return Ok(voivodeships);
    }
    
    // GET BY ID
    [HttpGet("{id}")]
    public ActionResult<Voivodeship> GetById([FromRoute]int id)
    {
        var voivodeship = _dbContext.Voivodeships.FirstOrDefault(v => v.Id == id);

        if (voivodeship is null)
        {
            NotFound();
        }
        
        return Ok(voivodeship);
    }
}