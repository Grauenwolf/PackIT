using PackIT.Infrastructure.Entities;
using PackIT.Infrastructure.ValueObjects;
using PackIT.Shared.Domain;

namespace PackIT.Infrastructure.Events
{
    public record PackingItemPacked(PackingList PackingList, PackingItem PackingItem) : IDomainEvent;
}
