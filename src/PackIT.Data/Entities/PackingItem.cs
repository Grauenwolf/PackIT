using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PackIT.Data.Entities
{
    public class PackingItem
    {
        public PackingItem()
        {
        }

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

        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public bool IsPacked { get; set; }

        public PackingList PackingList { get; set; }

        internal sealed class Configuration : IEntityTypeConfiguration<PackingItem>
        {
            public void Configure(EntityTypeBuilder<PackingItem> builder)
            {
                builder.ToTable("PackingItems");
            }
        }


    }

}
