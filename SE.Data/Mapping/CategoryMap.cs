using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.SeoUrl).HasMaxLength(120);
            builder.HasMany(d => d.Educations).WithOne(d => d.Category).HasForeignKey(d => d.CategoryId).IsRequired().OnDelete(DeleteBehavior.Cascade);

         
        }
    }
}
