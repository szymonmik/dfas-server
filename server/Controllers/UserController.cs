using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
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
    
    /// <summary>
    /// Creates new user account
    /// </summary>
    [HttpPost("register")]
    public ActionResult Register([FromBody]RegisterUserDto dto)
    {
        _userService.RegisterUser(dto);

        return Ok();
    }
    
    /// <summary>
    /// Returns succesfully authenticated user token
    /// </summary>
    [HttpPost("authenticate")]
    [Produces("application/json")]
    public ActionResult Authenticate([FromBody]LoginDto dto)
    {
        AuthenticationResponse authenticationResponse;
        authenticationResponse = _userService.GenerateJwt(dto);

        return Ok(authenticationResponse);
    }

    /// <summary>
    /// Gets authorized user data
    /// </summary>
    [HttpGet("{userId}")]
    [Authorize]
    public ActionResult GetUser([FromRoute]int userId)
    {
        var user = _userService.GetUser(userId, User);
        return Ok(user);
    }
    
    /// <summary>
    /// Updates authorized user region
    /// </summary>
    [HttpPost("{userId}/update/region")]
    [Authorize]
    public ActionResult UpdateRegion([FromRoute]int userId, [FromBody]UpdateUserRegionDto dto)
    {
        _userService.UpdateUserRegion(userId, dto, User);

        return NoContent();
    }

    /// <summary>
    /// Updates authorized user name
    /// </summary>
    [HttpPost("{userId}/update/name")]
    [Authorize]
    public ActionResult UpdateName([FromRoute] int userId, [FromBody] UpdateUserNameDto dto)
    {
        _userService.UpdateUserName(userId, dto, User);

        return NoContent();
    }

    /// <summary>
    /// Updates authorized user sex
    /// </summary>
    [HttpPost("{userId}/update/sex")]
    [Authorize]
    public ActionResult UpdateSex([FromRoute] int userId, [FromBody] UpdateUserSexDto dto)
    {
        _userService.UpdateUserSex(userId, dto, User);

        return NoContent();
    }

    /// <summary>
    /// Updates authorized user region, name and sex
    /// </summary>
    [HttpPost("{userId}/update")]
    [Authorize]
    public ActionResult Update([FromRoute] int userId, [FromBody] UpdateUserDto dto)
    {
        _userService.UpdateUser(userId, dto, User);

        return NoContent();
    }
    
    /// <summary>
    /// Updates authorized user password
    /// </summary>
    [HttpPost("{userId}/update/password")]
    [Authorize]
    public ActionResult Update([FromRoute] int userId, [FromBody] UpdateUserPasswordDto dto)
    {
        _userService.UpdateUserPassword(userId, dto, User);

        return NoContent();
    }
    
    /// <summary>
    /// Assigns allergen to own account
    /// </summary>
    [HttpPost("assignallergen/{allergenId}")]
    [Authorize]
    public ActionResult AssignAllergen([FromRoute] int allergenId)
    {
        _userService.AssignAllergen(allergenId, User);

        return NoContent();
    }
    
    /// <summary>
    /// Unassigns allergen from own account
    /// </summary>
    [HttpPost("unassignallergen/{allergenId}")]
    [Authorize]
    public ActionResult UnssignAllergen([FromRoute] int allergenId)
    {
        _userService.UnassignAllergen(allergenId, User);

        return NoContent();
    }
}