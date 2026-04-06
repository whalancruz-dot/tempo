using Microsoft.AspNetCore.Mvc;
using TempoApi.Business.Contract;
using TempoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserBussiness _userBussiness;

    public UserController(IUserBussiness userBussiness)
    {
        this._userBussiness = userBussiness;
    }

    [HttpPost("Create")]
    public IActionResult Create([FromBody] UserSiginRequest parametros)
    {
        return Ok(_userBussiness.CreateUser(parametros));
    }

}
