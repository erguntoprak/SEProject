using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationMap : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.ToTable("Education");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.SeoUrl).HasMaxLength(120);
            builder.HasOne(d => d.EducationAddress).WithOne(e => e.Education).HasForeignKey<EducationAddress>(f => f.EducationId);

            builder.HasMany(d => d.Images).WithOne(d => d.Education).HasForeignKey(d => d.EducationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Questions).WithOne(d => d.Education).HasForeignKey(d => d.EducationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
