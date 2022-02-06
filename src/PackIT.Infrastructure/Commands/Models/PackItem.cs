using System;

namespace PackIT.Infrastructure.Commands.Models
{
    public record PackItem(Guid PackingListId, string Name);
}
