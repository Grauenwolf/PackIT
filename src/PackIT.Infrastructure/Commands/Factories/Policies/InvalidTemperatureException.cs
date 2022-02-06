using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    public class InvalidTemperatureException : PackItException
    {
        public double Value { get; }

        public InvalidTemperatureException(double value) : base($"Value '{value}' is invalid temperature.")
            => Value = value;
    }
}