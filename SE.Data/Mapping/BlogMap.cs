using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class BlogMap : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blog");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Title).HasMaxLength(200).IsRequired();

            builder.HasMany(d => d.BlogItems).WithOne(d => d.Blog).HasForeignKey(d => d.BlogId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
