using Microsoft.EntityFrameworkCore;
using PackIT.Data.Entities;

namespace PackIT.Data
{
    public sealed class PackITDbContext : DbContext
    {
        public DbSet<PackingList> PackingLists { get; set; }

        public PackITDbContext(DbContextOptions<PackITDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("packing");

            modelBuilder.ApplyConfiguration(new PackingList.Configuration());
            modelBuilder.ApplyConfiguration(new PackingItem.Configuration());
        }
    }
}
