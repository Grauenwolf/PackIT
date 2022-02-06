using System.Threading.Tasks;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Models;

namespace PackIT.Infrastructure.Commands
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
