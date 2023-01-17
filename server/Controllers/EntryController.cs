using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Models;
using server.Services;

namespace server.Controllers;

[Route("api/entry/")]
[ApiController]
public class EntryController : ControllerBase
{
	private readonly IEntryService _entryService;

	public EntryController(IEntryService entryService)
	{
		_entryService = entryService;
	}
	
	/// <summary>
	/// Gets all own entries
	/// </summary>
	[HttpGet("own")]
	[Authorize]
	public ActionResult<IEnumerable<EntryDto>> GetAllCurrentUser()
	{
		var entries = _entryService.GetAllCurrentUser(User);
        
		return Ok(entries);
	}
    
	/// <summary>
	/// Gets own entry by id
	/// </summary>
	[HttpGet("{entryId}")]
	[Authorize]
	public ActionResult<EntryDto> GetById([FromRoute]int entryId)
	{
		var entry = _entryService.GetById(entryId, User);

		return Ok(entry);
	}

	/// <summary>
	/// Gets own entry by date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpGet("bydate/{entryDate}")]
	[Authorize]
	public ActionResult<EntryDto> GetByDate([FromRoute]string entryDate)
	{
		var entry = _entryService.GetByDate(entryDate, User);

		return Ok(entry);
	}

	/// <summary>
	/// Creates empty entry
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("empty")]
	[Authorize]
	public ActionResult CreateEmpty([FromBody] CreateEntryDto dto)
	{
		var id = _entryService.CreateEmptyEntry(dto, User);

		return Created($"/api/entry/{id}", null);
	}

	/// <summary>
	/// Deletes entry and existing assignments
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpDelete("{entryDate}")]
	[Authorize]
	public ActionResult Delete([FromRoute]string entryDate)
	{
		_entryService.DeleteEntry(entryDate, User);

		return Ok();
	}

	/// <summary>
	/// Assigns product to own entry with provided date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("{entryDate}/assign/product/{productId}")]
	[Authorize]
	public ActionResult AssignProduct([FromRoute] string entryDate, [FromRoute] int productId)
	{
		_entryService.AssignProduct(entryDate, productId, User);

		return Ok();
	}

	/// <summary>
	/// Unassigns product from own entry with provided date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("{entryDate}/unassign/product/{productId}")]
	[Authorize]
	public ActionResult UnassignProduct([FromRoute] string entryDate, [FromRoute] int productId)
	{
		_entryService.UnassignProduct(entryDate, productId, User);

		return Ok();
	}

	/// <summary>
	/// Assigns product to own entry with provided date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("{entryDate}/assign/symptom/{symptomId}")]
	[Authorize]
	public ActionResult AssignSymptom([FromRoute] string entryDate, [FromRoute] int symptomId)
	{
		_entryService.AssignSymptom(entryDate, symptomId, User);

		return Ok();
	}

	/// <summary>
	/// Unassigns product from own entry with provided date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("{entryDate}/unassign/symptom/{symptomId}")]
	[Authorize]
	public ActionResult UnassignSymptom([FromRoute] string entryDate, [FromRoute] int symptomId)
	{
		_entryService.UnassignSymptom(entryDate, symptomId, User);

		return Ok();
	}
}