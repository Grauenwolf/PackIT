using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PackIT.Data.Entities
{
    public class PackingList
    {
        public Guid Id { get; protected set; }
        public int Version { get; set; }

        public string Name { get; init; }
        public Localization Localization { get; init; }

        private readonly LinkedList<PackingItem> _items = new();

        private PackingList(Guid id, string name, Localization localization, LinkedList<PackingItem> items)
            : this(id, name, localization)
        {
            _items = items;
        }

        private PackingList()
        {
        }

        public PackingList(Guid id, string name, Localization localization)
        {
            Id = id;
            Name = name;
            Localization = localization;
        }

        public void AddItem(PackingItem item)
        {
            var alreadyExists = _items.Any(i => i.Name == item.Name);

            if (alreadyExists)
            {
                throw new PackingItemAlreadyExistsException(Name, item.Name);
            }

            _items.AddLast(item);
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
            var packedItem = item with { IsPacked = true };

            _items.Find(item).Value = packedItem;
        }

        public void RemoveItem(string itemName)
        {
            var item = GetItem(itemName);
            _items.Remove(item);
        }


        private PackingItem GetItem(string itemName)
        {
            var item = _items.SingleOrDefault(i => i.Name == itemName);

            if (item is null)
            {
                throw new PackingItemNotFoundException(itemName);
            }

            return item;
        }

        public void ThrowIfInvalid()
        {
            if (Id == Guid.Empty)
                throw new EmptyPackingListIdException();

            if (string.IsNullOrWhiteSpace(Name))
                throw new EmptyPackingListNameException();
        }

        internal sealed class Configuration : IEntityTypeConfiguration<PackingList>
        {
            public void Configure(EntityTypeBuilder<PackingList> builder)
            {
                builder.HasKey(pl => pl.Id);

                var localizationConverter = new ValueConverter<Localization, string>(l => l.ToString(),
                    l => Localization.Create(l));

                builder
                    .Property(pl => pl.Localization)
                    .HasConversion(localizationConverter)
                    .HasColumnName("Localization");

                builder.HasMany(typeof(PackingItem), "_items");

                builder.ToTable("PackingLists");
            }
        }
    }
}
