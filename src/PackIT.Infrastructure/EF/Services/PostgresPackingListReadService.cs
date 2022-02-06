using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackIT.Data;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Services;

namespace PackIT.Infrastructure.EF.Services
{
    internal sealed class PostgresPackingListReadService : IPackingListReadService
    {
        private readonly DbSet<PackingListReadModel> _packingList;

        public PostgresPackingListReadService(ReadDbContext context)
            => _packingList = context.PackingLists;

        public Task<bool> ExistsByNameAsync(string name)
            => _packingList.AnyAsync(pl => pl.Name == name);
    }
}