using System;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Factories.Policies;

namespace PackIT.Infrastructure.Commands.Factories
{
    public interface IPackingListFactory
    {
        PackingList Create(Guid id, string name, Localization localization);

        PackingList CreateWithDefaultItems(Guid id, string name, ushort days, Gender gender,
            double temperature, Localization localization);
    }
}
