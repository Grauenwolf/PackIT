using PackIT.Infrastructure.Consts;
using PackIT.Infrastructure.Entities;
using PackIT.Infrastructure.ValueObjects;

namespace PackIT.Infrastructure.Factories
{
    public interface IPackingListFactory
    {
        PackingList Create(PackingListId id, PackingListName name, Localization localization);

        PackingList CreateWithDefaultItems(PackingListId id, PackingListName name, TravelDays days, Gender gender,
            Temperature temperature, Localization localization);
    }
}
