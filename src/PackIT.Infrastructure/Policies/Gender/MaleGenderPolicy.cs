using PackIT.Infrastructure.ValueObjects;
using System;
using System.Collections.Generic;

namespace PackIT.Infrastructure.Policies.Gender
{
    internal sealed class MaleGenderPolicy : IPackingItemsPolicy
    {
        public bool IsApplicable(PolicyData data)
            => data.Gender is Consts.Gender.Male;

        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
            => new List<PackingItem>
            {
                new("Laptop", 1),
                new("Beer", 10),
                new("Book", (uint) Math.Ceiling(data.Days/7m)),
            };
    }
}