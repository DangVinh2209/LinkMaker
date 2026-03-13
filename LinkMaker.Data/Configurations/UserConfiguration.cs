using LinkMaker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManager.Common.Contants;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkMaker.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(MaxLengths.FULL_NAME);

            builder.Property(s => s.Avatar)
                .HasMaxLength(MaxLengths.AVATAR);
            builder.Property(s => s.Phone)
                .HasMaxLength(MaxLengths.PHONE);
            builder.Property(s => s.Email)
                .HasMaxLength(MaxLengths.EMAIL);
            builder.Property(s => s.Address)
                .HasMaxLength(MaxLengths.ADDRESS);
        }
    }
}
