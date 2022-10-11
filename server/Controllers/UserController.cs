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
    
    [HttpPost("register")]
    public ActionResult Register([FromBody]RegisterUserDto dto)
    {
        _userService.RegisterUser(dto);

        return Ok();
    }
    
    [HttpPost("login")]
    public ActionResult Login([FromBody]LoginDto dto)
    {
        string token = _userService.GenerateJwt(dto);

        return Ok(token);
    }
}