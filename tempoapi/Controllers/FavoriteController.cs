using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TempoApi.Business.Contract;
using TempoApi.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteBussiness _favoriteBussiness;
    public FavoriteController(IFavoriteBussiness favoriteBussiness)
    {
        _favoriteBussiness = favoriteBussiness;
    }

    [HttpGet]
    public IActionResult Buscar()
    {
        var todasClaims = User.Claims.Select(c => new { c.Type, c.Value });

        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("nameid")
                     ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized(new { mensagem = "Claim não encontrada", claims = todasClaims });

        return Ok(_favoriteBussiness.Buscar(usuarioId));
    }

    [HttpPost]
    public IActionResult Adicionar([FromBody] Favorite parametros)
    {

        var todasClaims = User.Claims.Select(c => new { c.Type, c.Value });

        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("nameid")
                     ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized(new { mensagem = "Claim não encontrada", claims = todasClaims });

        parametros.UsuarioId = usuarioId;

        _favoriteBussiness.Salvar(parametros);
        
        return Ok(new { mensagem = "Cidade favoritada com sucesso!" });
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(Guid id)
    {
        _favoriteBussiness.Remover(id);
        return Ok(new { mensagem = "Cidade removida dos favoritos!" });
    }

}