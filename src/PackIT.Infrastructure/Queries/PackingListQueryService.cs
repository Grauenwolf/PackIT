﻿using System.Collections.Generic;
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

        public Task<PackingListDto> HandleAsync(GetPackingList query)
        {
            return _packingLists
                .Include(pl => pl.Items)
                .Where(pl => pl.Id == query.Id)
                .Select(pl => pl.AsDto())
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<PackingListDto>> HandleAsync(SearchPackingLists query)
        {
            var dbQuery = _packingLists
                .Include(pl => pl.Items)
                .AsQueryable();

            if (query.SearchPhrase is not null)
            {
                dbQuery = dbQuery.Where(pl =>
                    Microsoft.EntityFrameworkCore.EF.Functions.ILike(pl.Name, $"%{query.SearchPhrase}%"));
            }

            return await dbQuery
                .Select(pl => pl.AsDto())
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
