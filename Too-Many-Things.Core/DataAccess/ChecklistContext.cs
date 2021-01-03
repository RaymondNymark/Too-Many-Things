using Microsoft.EntityFrameworkCore;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.DataAccess
{
    public class ChecklistContext : DbContext
    {
        public ChecklistContext()
        {

        }

        public ChecklistContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<List> Lists { get; set; }
        public DbSet<Entry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This should never be ran as the DBContext is not created by
                // anything without explicitly configuring it, and this it
                // should always be configured.
            }
        }
    }
}
