using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PackIT.Data.Entities
{
    public class PackingItemReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public bool IsPacked { get; set; }

        public PackingListReadModel PackingList { get; set; }

        internal sealed class Configuration : IEntityTypeConfiguration<PackingItemReadModel>
        {
            public void Configure(EntityTypeBuilder<PackingItemReadModel> builder)
            {
                builder.ToTable("PackingItems");
            }
        }
    }

}
