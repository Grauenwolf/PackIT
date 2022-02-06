using PackIT.Infrastructure.DTO.External;
using PackIT.Infrastructure.ValueObjects;
using System;
using System.Threading.Tasks;

namespace PackIT.Infrastructure.Services
{
    internal sealed class DumbWeatherService : IWeatherService
    {
        public Task<WeatherDto> GetWeatherAsync(Localization localization)
            => Task.FromResult(new WeatherDto(new Random().Next(5, 30)));
    }
}