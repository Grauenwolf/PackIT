using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    public class InvalidTravelDaysException : PackItException
    {
        public ushort Days { get; }

        public InvalidTravelDaysException(ushort days) : base($"Value '{days}' is invalid travel days.")
            => Days = days;
    }
}