using PackIT.Shared.Commands;
using System;

namespace PackIT.Application.Commands
{
    public record RemovePackingList(Guid Id) : ICommand;
}