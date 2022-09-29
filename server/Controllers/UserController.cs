using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("api/user/")]
public class UserController : ControllerBase
{
    // POST - Register User
    [HttpPost]
    public ActionResult Register()
    {
        throw new NotImplementedException();
    }
}