namespace TempoApi.Models
{
    public class WeatherResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public MainData Main { get; set; }
        public List<WeatherInfo> Weather { get; set; }
        public WindData Wind { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class WindData
    {
        public double Speed { get; set; }
    }
}