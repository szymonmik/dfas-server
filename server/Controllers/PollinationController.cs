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
	/// Not implemented yet
	/// </summary>
	[HttpGet]
	public IActionResult GetToday()
	{
		throw new NotImplementedException();
	}
}