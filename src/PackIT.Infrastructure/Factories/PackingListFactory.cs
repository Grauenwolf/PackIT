using PackIT.Infrastructure.Consts;
using PackIT.Infrastructure.Entities;
using PackIT.Infrastructure.Policies;
using PackIT.Infrastructure.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace PackIT.Infrastructure.Factories
{
    public sealed class PackingListFactory : IPackingListFactory
    {
        private readonly IEnumerable<IPackingItemsPolicy> _policies;

        public PackingListFactory(IEnumerable<IPackingItemsPolicy> policies)
            => _policies = policies;

        public PackingList Create(PackingListId id, PackingListName name, Localization localization)
            => new(id, name, localization);

        public PackingList CreateWithDefaultItems(PackingListId id, PackingListName name, TravelDays days, Gender gender,
            Temperature temperature, Localization localization)
        {
            var data = new PolicyData(days, gender, temperature, localization);
            var applicablePolicies = _policies.Where(p => p.IsApplicable(data));

            var items = applicablePolicies.SelectMany(p => p.GenerateItems(data));
            var packingList = Create(id, name, localization);

            packingList.AddItems(items);

            return packingList;
        }
    }
}
