using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackIT.Data;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Queries.Models;

namespace PackIT.Infrastructure.Queries
{
    public sealed class PackingListQueryService
    {
        private readonly DbSet<PackingList> _packingLists;

        public PackingListQueryService(PackITDbContext context)
            => _packingLists = context.PackingLists;

        public Task<PackingListDto> GetById(Guid id)
        {
            return _packingLists
                .Include(pl => pl.Items)
                .Where(pl => pl.Id == id)
                .Select(pl => pl.AsDto())
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<PackingListDto>> Search(string searchPhrase)
        {
            var dbQuery = _packingLists
                .Include(pl => pl.Items)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                dbQuery = dbQuery.Where(pl =>
                    Microsoft.EntityFrameworkCore.EF.Functions.ILike(pl.Name, $"%{searchPhrase}%"));
            }

            return await dbQuery
                .Select(pl => pl.AsDto())
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
