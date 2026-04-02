using Microsoft.AspNetCore.Mvc;
using TempoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;

    public WeatherController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = configuration.GetSection("OpenWeather")["ApiKey"];
    }

    [HttpGet("{cidade}")]
    public async Task<IActionResult> GetByCity(string cidade)
    {
        if (string.IsNullOrEmpty(_apiKey)) return StatusCode(500, "API Key não configurada no servidor.");

        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={_apiKey}&units=metric&lang=pt_br";
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) return BadRequest("Não foi possível encontrar a cidade ou erro na API.");

        var content = await response.Content.ReadFromJsonAsync<WeatherResponse>();

        return Ok(new
        {
            CidadeId = content.Id,
            Cidade = content.Name,
            Temperatura = content.Main.Temp.ToString("N1") + "°C",
            Clima = content.Weather[0].Description,
            Umidade = content.Main.Humidity + "%",
            Vento = content.Wind.Speed + " km/h",
            Icone = $"http://openweathermap.org/img/wn/{content.Weather[0].Icon}@2x.png"
        });
    }

    [HttpGet("previsao/{cidadeId}")]
    public async Task<IActionResult> GetForecast(int cidadeId)
    {
        if (string.IsNullOrEmpty(_apiKey)) return StatusCode(500, "API Key não configurada.");

        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.openweathermap.org/data/2.5/forecast?id={cidadeId}&appid={_apiKey}&units=metric&lang=pt_br";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) return BadRequest("Erro ao buscar previsão.");

        var content = await response.Content.ReadFromJsonAsync<ForecastResponse>();

        var previsaoDiaria = content.List
            .Where(x => x.Dt_txt.Contains("12:00:00"))
            .Select(x => new
            {
                Data = DateTime.Parse(x.Dt_txt).ToString("dd/MM"),
                DiaSemana = DateTime.Parse(x.Dt_txt).ToString("dddd"),
                Temperatura = x.Main.Temp.ToString("N0") + "°C",
                TempMin = x.Main.Temp_min.ToString("N0") + "°C",
                TempMax = x.Main.Temp_max.ToString("N0") + "°C",
                Clima = x.Weather[0].Description,
                Icone = x.Weather[0].Icon,
                Vento = x.Wind.Speed.ToString("N0") + " km/h", 
                Umidade = x.Main.Humidity + "%"              
            });

        return Ok(new
        {
            Cidade = content.City.Name,
            Previsoes = previsaoDiaria
        });
        
    }

}