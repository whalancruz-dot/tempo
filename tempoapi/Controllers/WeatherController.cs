using Microsoft.AspNetCore.Mvc;
using TempoApi.Business;
using TempoApi.Business.Contract;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWatherBussiness _weatherBusiness;

    public WeatherController(IWatherBussiness weatherBusiness)
    {
        _weatherBusiness = weatherBusiness;
    }

    [HttpGet("{cidade}")]
    public async Task<IActionResult> GetByCity(string cidade)
    {
        var resultado = await _weatherBusiness.GetByCityAsync(cidade);
        
        if (resultado == null) 
            return BadRequest("Não foi possível encontrar a cidade ou erro na API.");

        return Ok(resultado);
    }

    [HttpGet("previsao/{cidadeId}")]
    public async Task<IActionResult> GetForecast(int cidadeId)
    {
        var resultado = await _weatherBusiness.GetForecastAsync(cidadeId);

        if (resultado == null) 
            return BadRequest("Erro ao buscar previsão.");

        return Ok(resultado);
    }
}