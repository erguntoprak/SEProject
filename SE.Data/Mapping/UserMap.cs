using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirsName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);

            builder.HasMany(x => x.Educations).WithOne(y => y.User).HasForeignKey(b => b.UserId).IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
