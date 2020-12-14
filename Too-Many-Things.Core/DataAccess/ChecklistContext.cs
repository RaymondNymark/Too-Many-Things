using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
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
            // Lazy way of configuring DbContext.
            if (!optionsBuilder.IsConfigured)
            {
                // TODO : Store connection string in a safe place and let user
                // change it.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TooManyThingsDB;Trusted_Connection=True;");
            }
        }
    }
}
