﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PackIT.Data;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Models;

namespace PackIT.Infrastructure.Commands
{
    public sealed class PackingListCommandService : IPackingListCommandService
    {

        private readonly PackITDbContext _writeDbContext;

        public PackingListCommandService(PackITDbContext writeDbContext)
        {
            _writeDbContext = writeDbContext;
        }

        Task<PackingList> GetAsync(Guid id)
        {
            return _writeDbContext.PackingLists
                .Include(x => x.Items)
                .SingleOrDefaultAsync(pl => pl.Id == id);
        }

        async Task UpdateAsync(PackingList packingList)
        {
            packingList.ThrowIfInvalid();

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
            packingList.ThrowIfInvalid();
            packingList.Version = 1;

            await _writeDbContext.PackingLists.AddAsync(packingList);
            await _writeDbContext.SaveChangesAsync();

        }
    }
}
