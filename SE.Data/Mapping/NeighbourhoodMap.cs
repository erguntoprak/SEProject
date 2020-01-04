using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class NeighbourhoodMap : IEntityTypeConfiguration<Neighbourhood>
    {
        public void Configure(EntityTypeBuilder<Neighbourhood> builder)
        {
            builder.ToTable("Neighbourhood");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PostCode).HasMaxLength(20);
        }
    }
}
