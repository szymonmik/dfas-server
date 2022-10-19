using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/user/")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    // /api/user/register
    [HttpPost("register")]
    public ActionResult Register([FromBody]RegisterUserDto dto)
    {
        _userService.RegisterUser(dto);

        return Ok();
    }
    
    // /api/user/authenticate
    [HttpPost("authenticate")]
    public ActionResult Login([FromBody]LoginDto dto)
    {
        string token = _userService.GenerateJwt(dto);

        return Ok(token);
    }
    
    // /api/user/{id}/update/region
    [HttpPost("{id}/update/region")]
    [Authorize]
    public ActionResult UpdateRegion([FromRoute]int id, [FromBody]UpdateUserRegionDto dto)
    {
        _userService.UpdateUserRegion(id, dto, User);

        return NoContent();
    }

    // /api/user/{id}/update/name
    [HttpPost("{id}/update/name")]
    [Authorize]
    public ActionResult UpdateName([FromRoute] int id, [FromBody] UpdateUserNameDto dto)
    {
        _userService.UpdateUserName(id, dto, User);

        return NoContent();
    }

    // /api/user/{id}/update/sex
    [HttpPost("{id}/update/sex")]
    [Authorize]
    public ActionResult UpdateSex([FromRoute] int id, [FromBody] UpdateUserSexDto dto)
    {
        _userService.UpdateUserSex(id, dto, User);

        return NoContent();
    }
}