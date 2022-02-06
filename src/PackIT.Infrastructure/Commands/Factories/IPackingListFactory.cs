using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Factories.Policies;

namespace PackIT.Infrastructure.Commands.Factories
{
    public interface IPackingListFactory
    {
        PackingList Create(PackingListId id, PackingListName name, Localization localization);

        PackingList CreateWithDefaultItems(PackingListId id, PackingListName name, TravelDays days, Gender gender,
            Temperature temperature, Localization localization);
    }
}
