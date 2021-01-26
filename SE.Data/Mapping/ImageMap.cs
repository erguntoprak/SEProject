using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class ImageMap: IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");
            builder.HasKey(x => x.Id);

            builder.Property(d => d.ImageUrl).HasMaxLength(420).IsRequired();
            builder.Property(d => d.Title).HasMaxLength(400).IsRequired();

            builder.Property(d => d.FirstVisible).IsRequired();
            builder.Property(d => d.EducationId).IsRequired();
        }
    }
}
