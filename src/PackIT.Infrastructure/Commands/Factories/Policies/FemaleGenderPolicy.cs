﻿using System.Collections.Generic;
using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    internal sealed class FemaleGenderPolicy : IPackingItemsPolicy
    {
        public bool IsApplicable(PolicyData data)
            => data.Gender is Gender.Female;

        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
            => new List<PackingItem>
            {
                new("Lipstick", 1),
                new("Powder", 1),
                new("Eyeliner", 1)
            };
    }
}
