using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackIT.Data;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Exceptions;

namespace PackIT.Infrastructure.Commands.Handlers
{
    public sealed class PackingListCommandService : IPackingListCommandService
    {

        private readonly WriteDbContext _writeDbContext;

        public PackingListCommandService(WriteDbContext writeDbContext)
        {
            _writeDbContext = writeDbContext;
        }

        Task<PackingList> GetAsync(PackingListId id)
        {
            return _writeDbContext.PackingLists
                .Include("_items")
                .SingleOrDefaultAsync(pl => pl.Id == id);
        }

        async Task UpdateAsync(PackingList packingList)
        {
            packingList.Version += 1;
            _writeDbContext.PackingLists.Update(packingList);
            await _writeDbContext.SaveChangesAsync();
        }


        public async Task AddPackingItemAsync(AddPackingItem command)
        {
            var packingList = await GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            var packingItem = new PackingItem(command.Name, command.Quantity);
            packingList.AddItem(packingItem);

            await UpdateAsync(packingList);
        }

        public async Task PackItemAsync(PackItem command)
        {
            var packingList = await GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            packingList.PackItem(command.Name);

            await UpdateAsync(packingList);
        }

        public async Task RemovePackingItemAsync(RemovePackingItem command)
        {
            var packingList = await GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            packingList.RemoveItem(command.Name);

            await UpdateAsync(packingList);
        }

        public async Task RemovePackingListAsync(RemovePackingList command)
        {
            var packingList = await GetAsync(command.Id);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.Id);
            }

            _writeDbContext.PackingLists.Remove(packingList);
            await _writeDbContext.SaveChangesAsync();
        }
        public async Task AddPackingListAsync(PackingList packingList)
        {
            packingList.Version = 1;

            await _writeDbContext.PackingLists.AddAsync(packingList);
            await _writeDbContext.SaveChangesAsync();

        }
    }
}
