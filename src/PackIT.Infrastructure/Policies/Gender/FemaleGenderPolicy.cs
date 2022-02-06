using PackIT.Data.Entities;
using System.Collections.Generic;

namespace PackIT.Infrastructure.Policies.Gender
{
    internal sealed class FemaleGenderPolicy : IPackingItemsPolicy
    {
        public bool IsApplicable(PolicyData data)
            => data.Gender is Consts.Gender.Female;

        public IEnumerable<PackingItem> GenerateItems(PolicyData data)
            => new List<PackingItem>
            {
                new("Lipstick", 1),
                new("Powder", 1),
                new("Eyeliner", 1)
            };
    }
}