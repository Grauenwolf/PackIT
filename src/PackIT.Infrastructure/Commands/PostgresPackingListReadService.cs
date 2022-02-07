using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackIT.Data;
using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Commands
{
    internal sealed class PostgresPackingListReadService : IPackingListReadService
    {
        private readonly DbSet<PackingList> _packingList;

        public PostgresPackingListReadService(PackITDbContext context)
            => _packingList = context.PackingLists;

        public Task<bool> ExistsByNameAsync(string name)
            => _packingList.AnyAsync(pl => pl.Name == name);
    }
}
