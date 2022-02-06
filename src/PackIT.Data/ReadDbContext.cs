using Microsoft.EntityFrameworkCore;
using PackIT.Data.Entities;

namespace PackIT.Data
{
    public sealed class ReadDbContext : DbContext
    {
        public DbSet<PackingListReadModel> PackingLists { get; set; }

        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("packing");

            modelBuilder.ApplyConfiguration(new PackingListReadModel.Configuration());
            modelBuilder.ApplyConfiguration(new PackingItemReadModel.Configuration());
        }
    }
}
