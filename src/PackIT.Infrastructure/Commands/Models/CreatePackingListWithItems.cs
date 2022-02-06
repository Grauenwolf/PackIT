using System;
using PackIT.Infrastructure.Commands.Factories.Policies;

namespace PackIT.Infrastructure.Commands.Models
{
    public record CreatePackingListWithItems(Guid Id, string Name, ushort Days, Gender Gender,
        LocalizationWriteModel Localization);
}
