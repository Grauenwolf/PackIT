using System.Threading.Tasks;
using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Commands.Handlers
{
    public interface IPackingListCommandService
    {
        Task AddPackingListAsync(PackingList packingList);
        Task AddPackingItemAsync(AddPackingItem command);
        Task PackItemAsync(PackItem command);
        Task RemovePackingItemAsync(RemovePackingItem command);
        Task RemovePackingListAsync(RemovePackingList command);
    }
}
