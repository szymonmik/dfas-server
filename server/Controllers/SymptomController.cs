using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/symptom/")]
public class SymptomController : ControllerBase
{
	private readonly ISymptomService _symptomService;
	
	public SymptomController(ISymptomService symptomService)
	{
		_symptomService = symptomService;
	}
	
	/// <summary>
	/// Gets all symptoms
	/// </summary>
	[HttpGet]
	public ActionResult<IEnumerable<Symptom>> GetAll()
	{
		var symptoms = _symptomService.GetAll();

		return Ok(symptoms);
	}
    
	/// <summary>
	/// Gets symptom by id
	/// </summary>
	[HttpGet("{id}")]
	public ActionResult<Symptom> GetById([FromRoute]int id)
	{
		var symptom = _symptomService.GetById(id);

		return Ok(symptom);
	}
}