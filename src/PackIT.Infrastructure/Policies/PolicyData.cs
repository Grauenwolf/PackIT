using PackIT.Infrastructure.ValueObjects;

namespace PackIT.Infrastructure.Policies
{
    public record PolicyData(TravelDays Days, Consts.Gender Gender, ValueObjects.Temperature Temperature,
        Localization Localization);
}