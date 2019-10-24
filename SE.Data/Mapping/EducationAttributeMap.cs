using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationAttributeMap : IEntityTypeConfiguration<EducationAttribute>
    {
        public void Configure(EntityTypeBuilder<EducationAttribute> builder)
        {
            builder.ToTable("EducationAttribute");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100).IsRequired();


        }
    }
}
