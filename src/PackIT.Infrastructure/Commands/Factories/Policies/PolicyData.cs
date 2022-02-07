using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    public record PolicyData(ushort Days, Gender Gender, double Temperature,
        Localization Localization);
}
