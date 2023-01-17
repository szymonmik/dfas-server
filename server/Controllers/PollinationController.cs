using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Services;

namespace server.Controllers;

[Route("api/pollination/")]
[ApiController]
public class PollinationController : ControllerBase
{
	private readonly IPollinationService _pollinationService;

	public PollinationController(IPollinationService pollinationService)
	{
		_pollinationService = pollinationService;
	}

	/// <summary>
	/// Gets pollination by date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpGet("region/{regionId}/date/{date}")]
	public IActionResult GetByDate([FromRoute] int regionId, [FromRoute] string date)
	{
		var pollination = _pollinationService.GetByDate(regionId, date);

		return Ok(pollination);
	}

	/// <summary>
	/// Fills pollination with random strength on provided date
	/// </summary>
	/// <remarks>Date format: rrrr-mm-dd</remarks>
	[HttpPost("fill/{date}")]
	public IActionResult FillRandomOnDate(string date)
	{
		_pollinationService.FillRandomOnDate(date);

		return Ok();
	}
}