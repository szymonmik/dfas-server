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
    private readonly IUserService _userService;
    private readonly IMailService _mailService;

    public UserController(IUserService userService, IMailService mailService)
    {
        _userService = userService;
        _mailService = mailService;
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
        var authenticationResponse = _userService.GenerateJwt(dto);

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

        return Ok();
    }

    /// <summary>
    /// Updates authorized user name
    /// </summary>
    [HttpPost("{userId}/update/name")]
    [Authorize]
    public ActionResult UpdateName([FromRoute] int userId, [FromBody] UpdateUserNameDto dto)
    {
        _userService.UpdateUserName(userId, dto, User);

        return Ok();
    }

    /// <summary>
    /// Updates authorized user sex
    /// </summary>
    [HttpPost("{userId}/update/sex")]
    [Authorize]
    public ActionResult UpdateSex([FromRoute] int userId, [FromBody] UpdateUserSexDto dto)
    {
        _userService.UpdateUserSex(userId, dto, User);

        return Ok();
    }

    /// <summary>
    /// Updates authorized user region, name and sex
    /// </summary>
    [HttpPost("{userId}/update")]
    [Authorize]
    public ActionResult Update([FromRoute] int userId, [FromBody] UpdateUserDto dto)
    {
        _userService.UpdateUser(userId, dto, User);

        return Ok();
    }
    
    /// <summary>
    /// Updates authorized user password
    /// </summary>
    [HttpPost("{userId}/update/password")]
    [Authorize]
    public ActionResult Update([FromRoute] int userId, [FromBody] UpdateUserPasswordDto dto)
    {
        _userService.UpdateUserPassword(userId, dto, User);

        return Ok();
    }
    
    /// <summary>
    /// Assigns allergen to own account
    /// </summary>
    [HttpPost("assignallergen/{allergenId}")]
    [Authorize]
    public ActionResult AssignAllergen([FromRoute] int allergenId)
    {
        _userService.AssignAllergen(allergenId, User);

        return Ok();
    }
    
    /// <summary>
    /// Unassigns allergen from own account
    /// </summary>
    [HttpPost("unassignallergen/{allergenId}")]
    [Authorize]
    public ActionResult UnssignAllergen([FromRoute] int allergenId)
    {
        _userService.UnassignAllergen(allergenId, User);

        return Ok();
    }
    
    /// <summary>
    /// Send reset password email
    /// </summary>
    [HttpPost("forgotpassword")]
    public async Task<ActionResult> SendPasswordResetEmail([FromBody] ForgotPasswordDto dto)
    {
        var token = _userService.GeneratePasswordResetToken(dto);
        
        await _mailService.SendPasswordReset(dto, token);

        return Ok();
    }
    
    /// <summary>
    /// Resets password
    /// </summary>
    [HttpPost("resetpassword")]
    public ActionResult ResetPassword([FromBody] ResetPasswordDto dto)
    {
        _userService.ResetPassword(dto);

        return Ok();
    }
}