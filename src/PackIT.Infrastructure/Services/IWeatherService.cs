using PackIT.Infrastructure.DTO.External;
using PackIT.Infrastructure.ValueObjects;
using System.Threading.Tasks;

namespace PackIT.Infrastructure.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherAsync(Localization localization);
    }
}