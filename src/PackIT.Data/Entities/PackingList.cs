using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PackIT.Data.Entities
{
    public class PackingList
    {
        public PackingList()
        {
        }
        public PackingList(Guid id, string name, Localization localization)
        {
            Id = id;
            Name = name;
            Localization = localization;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public Localization Localization { get; set; }
        public List<PackingItem> Items { get; } = new();



        internal sealed class Configuration : IEntityTypeConfiguration<PackingList>
        {
            public void Configure(EntityTypeBuilder<PackingList> builder)
            {
                builder.ToTable("PackingLists");
                builder.HasKey(pl => pl.Id);

                builder
                    .Property(pl => pl.Localization)
                    .HasConversion(l => l.ToString(), l => Localization.Create(l));

                builder
                    .HasMany(pl => pl.Items)
                    .WithOne(pi => pi.PackingList);
            }

        }

        public void ThrowIfInvalid()
        {
            if (Id == Guid.Empty)
                throw new EmptyPackingListIdException();

            if (string.IsNullOrWhiteSpace(Name))
                throw new EmptyPackingListNameException();
        }

        public void AddItem(PackingItem item)
        {
            var alreadyExists = Items.Any(i => i.Name == item.Name);

            if (alreadyExists)
            {
                throw new PackingItemAlreadyExistsException(Name, item.Name);
            }

            Items.Add(item);
        }

        public void AddItems(IEnumerable<PackingItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void PackItem(string itemName)
        {
            var item = GetItem(itemName);
            item.IsPacked = true;
        }

        public void RemoveItem(string itemName)
        {
            var item = GetItem(itemName);
            Items.Remove(item);
        }

        private PackingItem GetItem(string itemName)
        {
            var item = Items.SingleOrDefault(i => i.Name == itemName);

            if (item is null)
            {
                throw new PackingItemNotFoundException(itemName);
            }

            return item;
        }
    }
}
