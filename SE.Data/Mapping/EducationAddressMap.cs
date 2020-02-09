using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationAddressMap: IEntityTypeConfiguration<EducationAddress>
    {
        public void Configure(EntityTypeBuilder<EducationAddress> builder)
        {
            builder.ToTable("EducationAddress");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.AddressOne).HasMaxLength(300).IsRequired();
        }
    }
}
