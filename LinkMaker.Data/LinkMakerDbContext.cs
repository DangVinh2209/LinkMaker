using Microsoft.EntityFrameworkCore;
//using LinkMaker.Common.Contants;
using LinkMaker.Data.Configurations;
using LinkMaker.Data.Entities;
using System;
//using LinkMaker.Common.Contants;
using StudentManager.Common.Contants;

namespace LinkMaker.Data
{
    public class LinkMakerDbContext : DbContext
    {
        //=== Step 1: Contructor ===//
        // Constructor dùng cho DI
        public LinkMakerDbContext(DbContextOptions<LinkMakerDbContext> options)
            : base(options)
        {
        }

        //=== Step 2: KHai báo DbSet ===//
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Url> Urls { get; set; }
        //public virtual DbSet<StudentClass> StudentClasses { get; set; }

        //=== Step 3: OnConfiguring ===//
        // Dùng khi KHÔNG cấu hình trong Program.cs
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(
            //        "Server=localhost;Database=StudentDb;Trusted_Connection=True;TrustServerCertificate=True");
            //}
        }

        //=== Step 4: OnModelCreating ===//
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentManagerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new StudentClassConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.Entity<User>()
           .HasOne(u => u.Url)
           .WithMany(u => u.Users)
           .HasForeignKey(u => u.UrlId);

            // ===== Student =====
            //modelBuilder.Entity<Student>(entity =>
            //{
            //    //entity.ToTable("Students");
            //    //entity.HasKey(e => e.Id);

            //    entity.Property(e => e.FullName)
            //          .IsRequired()
            //          .HasMaxLength(MaxLengths.FULL_NAME);

            //    entity.Property(e => e.Phone)
            //          .HasMaxLength(15);
            //    entity.Property(e => e.Email)
            //          .HasMaxLength(250);
            //    entity.Property(e => e.Address)
            //          .HasMaxLength(500);

            //    //entity.Property(e => e.Age)
            //    //      .HasDefaultValue(18);
            //});

            // ===== Course =====
            modelBuilder.Entity<Url>(entity =>
            {
                //entity.ToTable("Majors");
                //entity.HasKey(e => e.Id);

                entity.Property(e => e.YourLink)
                      .IsRequired()
                      .HasMaxLength(MaxLengths.TITLE);
                //entity.Property(e => e.NewLink)
                //      .HasMaxLength(MaxLengths.DESCRIPTION);
                entity.Property(e => e.UrlCode)
                      .HasMaxLength(15);
            });

        }
    }
}
