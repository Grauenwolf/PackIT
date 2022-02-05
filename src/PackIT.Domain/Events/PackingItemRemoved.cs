using PackIT.Domain.Entities;
using PackIT.Domain.ValueObjects;
using PackIT.Shared.Domain;

namespace PackIT.Domain.Events
{
    public record PackingItemRemoved(PackingList PackingList, PackingItem PackingItem) : IDomainEvent;
}