using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class DistrictMap : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("District");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.SeoUrl).HasMaxLength(50).IsRequired();

            builder.HasMany(d => d.EducationAddress).WithOne(d => d.District).HasForeignKey(d => d.DistrictId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
