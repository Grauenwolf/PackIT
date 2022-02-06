using System;

namespace PackIT.Infrastructure.Commands
{
    public record RemovePackingItem(Guid PackingListId, string Name);
}
