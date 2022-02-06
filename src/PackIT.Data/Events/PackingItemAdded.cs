﻿using PackIT.Data.Entities;
using PackIT.Shared.Domain;

namespace PackIT.Data.Events
{
    public record PackingItemAdded(PackingList PackingList, PackingItem PackingItem) : IDomainEvent;
}