using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected string? UsuarioId => 
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
        User.FindFirstValue("nameid") ?? 
        User.FindFirstValue("sub");

    protected bool UsuarioAutenticado => !string.IsNullOrEmpty(UsuarioId);
}