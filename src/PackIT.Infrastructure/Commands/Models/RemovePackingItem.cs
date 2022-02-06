using System;

namespace PackIT.Infrastructure.Commands.Models
{
    public record RemovePackingItem(Guid PackingListId, string Name);
}
