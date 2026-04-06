using Microsoft.AspNetCore.Mvc;
using TempoApi.Business.Contract;
using TempoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthBussiness _authBussiness;

    public AuthController(IAuthBussiness authBussiness)
    {
        this._authBussiness = authBussiness;
    }

    [HttpGet("GetToken")]
    public IActionResult GetToken([FromQuery] UserLoginRequest parametros)
    {
        return Ok(_authBussiness.Authentication(parametros));
    }

}
