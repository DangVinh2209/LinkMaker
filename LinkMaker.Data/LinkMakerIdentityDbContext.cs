using LinkMaker.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkMaker.Data
{
    public class LinkMakerIdentityDbContext : IdentityDbContext
    {
        public LinkMakerIdentityDbContext(DbContextOptions<LinkMakerIdentityDbContext> options)
            : base(options)
        {
        }
        public DbSet<LinkMakerUser> linkMakerUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LinkMakerUser>()
                .Property(p => p.FullName);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
