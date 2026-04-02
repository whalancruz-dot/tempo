using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using TempoApi.Business.Contract;
using TempoApi.Models;

namespace TempoApi.Business
{
    public class WeatherBusiness : IWatherBussiness
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public WeatherBusiness(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration.GetSection("OpenWeather")["ApiKey"]
                      ?? throw new ArgumentNullException("API Key não configurada.");
        }

        public async Task<object?> GetByCityAsync(string cidade)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={_apiKey}&units=metric&lang=pt_br";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            if (content == null) return null;

            return new
            {
                CidadeId = content.Id,
                Cidade = content.Name,
                Temperatura = content.Main.Temp.ToString("N1") + "°C",
                Clima = content.Weather[0].Description,
                Umidade = content.Main.Humidity + "%",
                Vento = content.Wind.Speed + " km/h",
                Icone = $"http://openweathermap.org/img/wn/{content.Weather[0].Icon}@2x.png"
            };
        }

        public async Task<object?> GetForecastAsync(int cidadeId)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.openweathermap.org/data/2.5/forecast?id={cidadeId}&appid={_apiKey}&units=metric&lang=pt_br";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadFromJsonAsync<ForecastResponse>();
            if (content == null) return null;

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

            return new
            {
                Cidade = content.City.Name,
                Previsoes = previsaoDiaria
            };
        }
    }
}