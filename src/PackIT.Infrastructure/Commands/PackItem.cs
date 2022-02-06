using System;

namespace PackIT.Infrastructure.Commands
{
    public record PackItem(Guid PackingListId, string Name);
}
