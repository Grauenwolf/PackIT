
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PackIT.Data.Entities
{
    public record PackingItem
    {
        public string Name { get; }
        public uint Quantity { get; }
        public bool IsPacked { get; init; }

        public PackingItem(string name, uint quantity, bool isPacked = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EmptyPackingListItemNameException();
            }

            Name = name;
            Quantity = quantity;
            IsPacked = isPacked;
        }

        internal sealed class Configuration : IEntityTypeConfiguration<PackingItem>
        {
            public void Configure(EntityTypeBuilder<PackingItem> builder)
            {
                builder.Property<Guid>("Id");
                builder.Property(pi => pi.Name);
                builder.Property(pi => pi.Quantity);
                builder.Property(pi => pi.IsPacked);
                builder.ToTable("PackingItems");
            }
        }
    }
}
