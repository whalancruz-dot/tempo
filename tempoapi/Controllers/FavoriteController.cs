using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TempoApi.Business.Contract;
using TempoApi.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoriteController : ApiBaseController
{
    private readonly IFavoriteBussiness _favoriteBussiness;
    public FavoriteController(IFavoriteBussiness favoriteBussiness)
    {
        _favoriteBussiness = favoriteBussiness;
    }

    [HttpGet]
    public IActionResult Buscar()
    {
        if (!UsuarioAutenticado) return Unauthorized("Usuário não identificado.");

        return Ok(_favoriteBussiness.Buscar(UsuarioId!));
    }

    [HttpPost]
    public IActionResult Adicionar([FromBody] Favorite parametros)
    {
        if (!UsuarioAutenticado) return Unauthorized("Usuário não identificado.");

        parametros.UsuarioId = UsuarioId!;

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