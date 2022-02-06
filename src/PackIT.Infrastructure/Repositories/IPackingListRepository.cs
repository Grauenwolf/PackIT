using PackIT.Infrastructure.Entities;
using PackIT.Infrastructure.ValueObjects;
using System.Threading.Tasks;

namespace PackIT.Infrastructure.Repositories
{
    public interface IPackingListRepository
    {
        Task<PackingList> GetAsync(PackingListId id);
        Task AddAsync(PackingList packingList);
        Task UpdateAsync(PackingList packingList);
        Task DeleteAsync(PackingList packingList);
    }
}