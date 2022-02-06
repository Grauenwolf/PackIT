using PackIT.Shared.Exceptions;
using System;

namespace PackIT.Infrastructure.Exceptions
{
    public class PackingListNotFoundException : PackItException
    {
        public Guid Id { get; }

        public PackingListNotFoundException(Guid id) : base($"Packing list with ID '{id}' was not found.")
            => Id = id;
    }
}