namespace TempoApi.Models
{

    public class ForecastResponse
    {
        public List<ForecastItem> List { get; set; }
        public CityInfo City { get; set; }
    }

    public class ForecastItem
    {
        public string Dt_txt { get; set; }
        public MainInfo Main { get; set; }
        public List<WeatherInfo> Weather { get; set; }
        public WindInfo Wind { get; set; }   
    }

    public class MainInfo
    {
        public float Temp { get; set; }
        public float Temp_min { get; set; }     
        public float Temp_max { get; set; }      
        public int Humidity { get; set; }         
    }

    public class WindInfo                        
    {
        public float Speed { get; set; }
    }

    public class CityInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}