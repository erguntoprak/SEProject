using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class BlogItemMap : IEntityTypeConfiguration<BlogItem>
    {
        public void Configure(EntityTypeBuilder<BlogItem> builder)
        {
            builder.ToTable("BlogItem");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.ImageName).HasMaxLength(200).IsRequired();
        }
    }
}
