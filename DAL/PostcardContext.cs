﻿using System.Data.Entity;
using PostcardsManager.Models;

namespace PostcardsManager.DAL
{
    public class PostcardContext : DbContext
    {
        public DbSet<Series> Series { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Postcard> Postcards { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Storage> Storages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Series>()
                .HasRequired(h => h.Publisher)
                .WithMany(w => w.Series)
                .HasForeignKey(k => k.PublisherId);

            modelBuilder.Entity<Image>()
                .HasRequired(h => h.Storage)
                .WithMany(w => w.Images)
                .HasForeignKey(k => k.StorageId);

            modelBuilder.Entity<Postcard>()
                .HasOptional(c => c.Series)
                .WithMany(c => c.Postcards)
                .HasForeignKey(k => k.SeriesId);

            modelBuilder.Entity<Postcard>()
                .HasOptional(h => h.Photographer)
                .WithMany(w => w.Postcards)
                .HasForeignKey(k => k.PhotographerId);

            modelBuilder.Entity<Postcard>()
                .HasOptional(h => h.ImageFront);
            
            modelBuilder.Entity<Postcard>()
                .HasOptional(h => h.ImageBack);
        }
    }
}