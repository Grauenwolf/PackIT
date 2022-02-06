using PackIT.Data.Entities;
using PackIT.Infrastructure.DTO.External;
using System.Threading.Tasks;

namespace PackIT.Infrastructure.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherAsync(Localization localization);
    }
}