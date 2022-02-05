using PackIT.Shared.Commands;
using System;

namespace PackIT.Application.Commands
{
    public record RemovePackingItem(Guid PackingListId, string Name) : ICommand;
}