using System;

namespace PackIT.Infrastructure.Commands
{
    public record AddPackingItem(Guid PackingListId, string Name, uint Quantity);
}
