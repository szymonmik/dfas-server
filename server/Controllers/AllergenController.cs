
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
	
	/// <summary>
	/// Gets all allergens
	/// </summary>
	[HttpGet]
	public ActionResult<IEnumerable<Allergen>> GetAllergens()
	{
		var allergens = _allergenService.GetAllAllergens();

		return Ok(allergens);
	}
    
	/// <summary>
	/// Gets allergen by id
	/// </summary>
	[HttpGet("{allergenId}")]
	public ActionResult<Allergen> GetAllergenById([FromRoute]int id)
	{
		var allergen = _allergenService.GetAllergenById(id);

		return Ok(allergen);
	}
	
	/// <summary>
	/// Gets all allergen types
	/// </summary>
	[HttpGet("type")]
	public ActionResult<IEnumerable<AllergenType>> GetAllergenTypes()
	{
		var allergenTypes = _allergenService.GetAllAllergenTypes();

		return Ok(allergenTypes);
	}
	
	/// <summary>
	/// Gets allergen type by id
	/// </summary>
	[HttpGet("type/{allergenTypeId}")]
	public ActionResult<AllergenType> GetAllergenTypeById([FromRoute]int id)
	{
		var allergenType = _allergenService.GetAllergenTypeById(id);

		return Ok(allergenType);
	}
}

