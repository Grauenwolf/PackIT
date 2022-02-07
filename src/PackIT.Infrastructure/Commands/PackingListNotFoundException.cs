using PackIT.Shared.Exceptions;
using System;

namespace PackIT.Infrastructure.Commands
{
    public class PackingListNotFoundException : BadRequestException
    {
        public Guid Id { get; }

        public PackingListNotFoundException(Guid id) : base($"Packing list with ID '{id}' was not found.")
            => Id = id;
    }
}