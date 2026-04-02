using Microsoft.AspNetCore.Mvc;
using TempoApi.Business.Contract;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthBussiness _authBussiness;

    public AuthController(IConfiguration config, IAuthBussiness authBussiness)
    {
        _config = config;

        this._authBussiness = authBussiness;
    } 

    [HttpGet("GetToken")]
    public IActionResult GetToken()
    {
        string autoUuid = Guid.NewGuid().ToString();
        var token = _authBussiness.GerarToken(autoUuid);
        return Ok(new { token });
    }

}
