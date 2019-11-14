using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class AttributeCategoryMap : IEntityTypeConfiguration<AttributeCategory>
    {
        public void Configure(EntityTypeBuilder<AttributeCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.HasMany(x => x.Attributes).WithOne(y=>y.AttributeCategory).HasForeignKey(b => b.AttributeCategoryId).IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
