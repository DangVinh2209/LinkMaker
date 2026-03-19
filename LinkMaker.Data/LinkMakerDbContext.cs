using Microsoft.EntityFrameworkCore;
using LinkMaker.Data.Configurations;
using LinkMaker.Data.Entities;
using System;
using StudentManager.Common.Contants;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply external configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // === THE FIX: ONE USER HAS MANY URLS ===
            // This explicitly links the ICollection<Url> in User to the UserId in Url
            modelBuilder.Entity<Url>(entity =>
            {
                entity.Ignore("Users");
                // A Url has ONE User
                entity.HasOne(d => d.User)
                    // A User has MANY Urls
                    .WithMany(p => p.Urls)
                    // The Foreign Key is on the Url table (UserId)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Existing property configurations
                entity.Property(e => e.YourLink)
                      .IsRequired()
                      .HasMaxLength(MaxLengths.TITLE);

                //entity.Property(e => e.UrlCode)
                //      .HasMaxLength(15);
            });

            // If you still have the old mapping code under modelBuilder.Entity<User>(), 
            // DELETE IT to avoid conflicts.
        }
    }
}