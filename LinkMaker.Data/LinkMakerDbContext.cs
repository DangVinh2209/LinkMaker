using Microsoft.EntityFrameworkCore;
using LinkMaker.Data.Configurations;
using LinkMaker.Data.Entities;
using System;
using LinkMaker.Common.Contants;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LinkMaker.Data
{
    public class LinkMakerDbContext : DbContext
    {
        public LinkMakerDbContext(DbContextOptions<LinkMakerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Url> Urls { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }

        // === ADD THIS METHOD TO FIX THE PENDING CHANGES ERROR ===
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.ConfigureWarnings(w =>
        //        w.Ignore(RelationalEventId.PendingModelChangesWarning));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<Url>(entity =>
            {
                entity.Ignore("Users");
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.YourLink)
                      .IsRequired()
                      .HasMaxLength(MaxLengths.TITLE);
            });

            modelBuilder.Entity<QRCode>(entity =>
            {
                entity.Property(e => e.URL)
                      .IsRequired()
                      .HasMaxLength(MaxLengths.URL);
                entity.Property(e => e.FileName)
                      .HasMaxLength(MaxLengths.TITLE);
            });
        }
    }
}