using System.Linq;
using Microsoft.EntityFrameworkCore;
using PackIT.Data.Entities;

namespace PackIT.Data
{
    public sealed class WriteDbContext : DbContext
    {
        public DbSet<PackingList> PackingLists { get; set; }

        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
        {
            SavingChanges += WriteDbContext_SavingChanges;
        }

        /// <summary>
        /// This is invoked at the beginning of Save or SaveAsync.
        /// </summary>
        private void WriteDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            foreach (var item in ChangeTracker.Entries().OfType<IHasVersion>())
                item.Version += 1;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("packing");

            modelBuilder.ApplyConfiguration(new PackingList.Configuration());
            modelBuilder.ApplyConfiguration(new PackingItem.Configuration());
        }


    }
}
