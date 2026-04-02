using Microsoft.Extensions.DependencyInjection;
using TempoApi.Business;
using TempoApi.Business.Contract;
using TempoApi.Repository;

namespace TempoApi.IoC
{
    public static class DIConfiguration
    {
        public static void AddApplicationRegistry(this IServiceCollection services)
        {
            services.AddScoped<IAuthBussiness, AuthBussiness>();
            services.AddScoped<IFavoriteBussiness, FavoriteBussiness>();
            services.AddScoped<IWatherBussiness, WeatherBusiness>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            
        }


    }
}
