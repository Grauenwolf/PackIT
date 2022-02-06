using PackIT.Data.Entities;
using System.Collections.Generic;

namespace PackIT.Infrastructure.Commands.Factories.Policies
{
    public interface IPackingItemsPolicy
    {
        bool IsApplicable(PolicyData data);
        IEnumerable<PackingItem> GenerateItems(PolicyData data);
    }
}