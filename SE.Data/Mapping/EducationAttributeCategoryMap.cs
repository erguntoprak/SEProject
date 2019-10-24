using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationAttributeCategoryMap : IEntityTypeConfiguration<EducationAttributeCategory>
    {
        public void Configure(EntityTypeBuilder<EducationAttributeCategory> builder)
        {
            builder.ToTable("EducationAttributeCategory");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.HasMany(x => x.EducationAttributes).WithOne(y=>y.EducationAttributeCategory).HasForeignKey(b => b.EducationAttributeCategoryId).IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
