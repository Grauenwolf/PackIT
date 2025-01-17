﻿using System;
using System.Collections.Generic;
using System.Linq;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Factories.Policies;

namespace PackIT.Infrastructure.Commands.Factories
{
    public sealed class PackingListFactory : IPackingListFactory
    {
        private readonly IEnumerable<IPackingItemsPolicy> _policies;

        public PackingListFactory(IEnumerable<IPackingItemsPolicy> policies)
            => _policies = policies;

        public PackingList Create(Guid id, string name, Localization localization)
            => new(id, name, localization);

        public PackingList CreateWithDefaultItems(Guid id, string name, ushort days, Gender gender,
            double temperature, Localization localization)
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
