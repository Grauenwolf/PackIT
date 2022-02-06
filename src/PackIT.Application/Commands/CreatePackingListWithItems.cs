using System;
using PackIT.Domain.Consts;

namespace PackIT.Application.Commands
{
    public record CreatePackingListWithItems(Guid Id, string Name, ushort Days, Gender Gender,
        LocalizationWriteModel Localization);

    public record LocalizationWriteModel(string City, string Country);
}
