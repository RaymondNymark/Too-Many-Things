using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Too_Many_Things.Core.Models
{
    public partial class TooManyThingsContext : DbContext
    {
        public TooManyThingsContext()
        {
        }

        public TooManyThingsContext(DbContextOptions<TooManyThingsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Checklist> Checklist { get; set; }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public virtual DbSet<Entry> Entry { get; set; }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Enables lazy loading of EF-core entities
                optionsBuilder.UseLazyLoadingProxies();
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TooManyThings;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checklist>(entity =>
            {
                entity.Property(e => e.ChecklistId).HasColumnName("ChecklistID");
                entity.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            });

            modelBuilder.Entity<Entry>(entity =>
            {
                entity.Property(e => e.ChecklistId).HasColumnName("ChecklistID");
                entity.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

                entity.HasOne(d => d.Checklist)
                    .WithMany(p => p.Entry)
                    .HasForeignKey(d => d.ChecklistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entries_ToTable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
