using System;

namespace PackIT.Infrastructure.Commands.Models
{
    public record AddPackingItem(Guid PackingListId, string Name, uint Quantity);
}
