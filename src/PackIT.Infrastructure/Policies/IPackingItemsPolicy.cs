using PackIT.Infrastructure.ValueObjects;
using System.Collections.Generic;

namespace PackIT.Infrastructure.Policies
{
    public interface IPackingItemsPolicy
    {
        bool IsApplicable(PolicyData data);
        IEnumerable<PackingItem> GenerateItems(PolicyData data);
    }
}