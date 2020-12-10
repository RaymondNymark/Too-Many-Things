//using Too_Many_Things.Core.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Data;

//namespace Too_Many_Things.Core.DbContext
//{
//    public class ChecklistContext : Microsoft.EntityFrameworkCore.DbContext
//    {
//        // TODO : Absolutely get rid of this.
//        public static readonly string DefaultConnection = "Server=(localdb)\\MSSQLLocalDB;Database=TooManyThings;Trusted_Connection=True;";


//        public ChecklistContext(DbContextOptions<ChecklistContext> options = DefaultConnection) : base(options)
//        {
//        }

//        public DbSet<List> Lists { get; set; }
//        public DbSet<Entry> Entries { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // Overriding default pluralization of names to keep it to singular.
//            modelBuilder.Entity<List>().ToTable("List");
//            modelBuilder.Entity<Entry>().ToTable("Entry");


//            //modelBuilder.Entity<List>()
//            //    .HasMany(c => c.Entries)
//            //    .WithOne(e => e.List);
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning TODO : REMOVE THIS CONNECTION STRING
//                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TooManyThings;Trusted_Connection=True;");
//            }
//        }
//    }
//}
