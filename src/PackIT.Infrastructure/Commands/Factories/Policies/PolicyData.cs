using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    public record PolicyData(TravelDays Days, Gender Gender, Temperature Temperature,
        Localization Localization);
}
