using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PackIT.Data.Entities
{
    public class PackingListReadModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public LocalizationReadModel Localization { get; set; }
        public ICollection<PackingItemReadModel> Items { get; set; }



        internal sealed class Configuration : IEntityTypeConfiguration<PackingListReadModel>
        {
            public void Configure(EntityTypeBuilder<PackingListReadModel> builder)
            {
                builder.ToTable("PackingLists");
                builder.HasKey(pl => pl.Id);

                builder
                    .Property(pl => pl.Localization)
                    .HasConversion(l => l.ToString(), l => LocalizationReadModel.Create(l));

                builder
                    .HasMany(pl => pl.Items)
                    .WithOne(pi => pi.PackingList);
            }

        }
    }
}
