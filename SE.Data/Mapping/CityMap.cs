using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).HasMaxLength(50).IsRequired();

            builder.HasMany(d => d.Districts).WithOne(d => d.City).HasForeignKey(d => d.CityId).IsRequired().OnDelete(DeleteBehavior.Cascade);


        }
    }
}
