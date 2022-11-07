
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Services;

namespace server.Controllers;

[Route("api/allergen/")]
[ApiController]
public class AllergenController : ControllerBase
{
	private readonly IAllergenService _allergenService;

	public AllergenController(IAllergenService allergenService)
	{
		_allergenService = allergenService;
	}
	
	// GET ALL ALLERGENS
	[HttpGet]
	public ActionResult<IEnumerable<Allergen>> GetAllergens()
	{
		var allergens = _allergenService.GetAllAllergens();

		return Ok(allergens);
	}
    
	// GET ALLERGEN BY ID
	[HttpGet("{id}")]
	public ActionResult<Allergen> GetAllergenById([FromRoute]int id)
	{
		var allergen = _allergenService.GetAllergenById(id);

		if (allergen is null)
		{
			return NotFound();
		}
        
		return Ok(allergen);
	}
	
	// GET ALL ALLERGEN TYPES
	[HttpGet("type")]
	public ActionResult<IEnumerable<AllergenType>> GetAllergenTypes()
	{
		var allergenTypes = _allergenService.GetAllAllergenTypes();

		return Ok(allergenTypes);
	}
	
	// GET ALLERGEN TYPE BY ID
	[HttpGet("type/{id}")]
	public ActionResult<AllergenType> GetAllergenTypeById([FromRoute]int id)
	{
		var allergenType = _allergenService.GetAllergenTypeById(id);

		if (allergenType is null)
		{
			return NotFound();
		}
        
		return Ok(allergenType);
	}
}

