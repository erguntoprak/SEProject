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

            builder.Property(d => d.Title).HasMaxLength(400).IsRequired();
            builder.Property(d => d.SeoUrl).HasMaxLength(450).IsRequired();
            builder.Property(d => d.FirstVisibleImageName).HasMaxLength(450).IsRequired();
            builder.Property(d => d.UserId).HasMaxLength(50).IsRequired();
            builder.Property(d => d.CreateTime).IsRequired();
            builder.Property(d => d.UpdateTime).IsRequired();
            builder.Property(d => d.MetaKeywords).HasMaxLength(400);
            builder.Property(d => d.MetaTitle).HasMaxLength(400);
            builder.Property(d => d.IsActive).IsRequired();


            builder.HasMany(d => d.BlogItems).WithOne(d => d.Blog).HasForeignKey(d => d.BlogId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
