

namespace TempoApi.Business.Contract
{
    public interface IWatherBussiness
    {
        Task<object?> GetByCityAsync(string cidade);
        Task<object?> GetForecastAsync(int cidadeId);
    }
}

