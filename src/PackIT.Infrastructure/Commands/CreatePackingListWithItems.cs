using PackIT.Infrastructure.Consts;
using System;

namespace PackIT.Infrastructure.Commands
{
    public record CreatePackingListWithItems(Guid Id, string Name, ushort Days, Gender Gender,
        LocalizationWriteModel Localization);

    public record LocalizationWriteModel(string City, string Country);
}
