using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    // Retorna o Guid se o claim existir e for válido, caso contrário retorna Guid.Empty ou null
    protected Guid UsuarioId
    {
        get
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
                           User.FindFirstValue("nameid") ?? 
                           User.FindFirstValue("sub");

            if (Guid.TryParse(idString, out Guid guidId))
            {
                return guidId;
            }

            return Guid.Empty;
        }
    }

    protected bool UsuarioAutenticado => UsuarioId != Guid.Empty;
}