using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PostcardsManager.Models;

namespace PostcardsManager.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Series> Series { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Postcard> Postcards { get; set; }
        public DbSet<Photographer> Photographers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Series>()
                .HasRequired(h => h.Publisher)
                .WithMany(w => w.Series)
                .HasForeignKey(k => k.PublisherId);

            modelBuilder.Entity<Postcard>()
                .HasRequired(c => c.Series)
                .WithMany(c => c.Postcards)
                .HasForeignKey(k => k.SeriesId);

            modelBuilder.Entity<Postcard>()
                .HasOptional(h => h.Photographer)
                .WithMany(w => w.Postcards)
                .HasForeignKey(k => k.PhotographerId);
        }
    }
}