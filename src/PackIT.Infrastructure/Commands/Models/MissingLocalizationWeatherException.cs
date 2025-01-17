using PackIT.Data.Entities;
using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Commands.Models
{
    public class MissingLocalizationWeatherException : BadRequestException
    {
        public Localization Localization { get; }

        public MissingLocalizationWeatherException(Localization localization) 
            : base($"Couldn't fetch weather data for localization '{localization.Country}/{localization.City}'.")
        {
            Localization = localization;
        }
    }
}