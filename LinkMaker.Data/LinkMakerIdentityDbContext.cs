using LinkMaker.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.Contants;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkMaker.Data
{
    public class LinkMakerIdentityDbContext : IdentityDbContext<LinkMakerUser, UserManagerRole, string>
    {
        public LinkMakerIdentityDbContext(DbContextOptions<LinkMakerIdentityDbContext> options)
            : base(options)
        {
        }
        public DbSet<LinkMakerUser> linkMakerUsers { get; set; }
        public DbSet<UserManagerRole> UserManagerRole { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LinkMakerUser>()
                .Property(p => p.FullName)
                .HasMaxLength(MaxLengths.FULL_NAME);
            modelBuilder.Entity<LinkMakerUser>()
               .Property(p => p.Avatar)
               .HasMaxLength(MaxLengths.AVATAR);

            modelBuilder.Entity<LinkMakerUser>()
                .Property(p => p.Description)
                .HasMaxLength(MaxLengths.DESCRIPTION);


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
