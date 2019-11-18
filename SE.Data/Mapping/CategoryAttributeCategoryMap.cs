using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class CategoryAttributeCategoryMap : IEntityTypeConfiguration<CategoryAttributeCategory>
    {
        public void Configure(EntityTypeBuilder<CategoryAttributeCategory> builder)
        {

            builder.HasKey(x => x.Id);

            builder.HasOne(mapping => mapping.Category)
                 .WithMany(discount => discount.CategoryAttributeCategories)
                 .HasForeignKey(mapping => mapping.CategoryId)
                 .IsRequired();

            builder.HasOne(mapping => mapping.AttributeCategory)
                .WithMany(category => category.CategoryAttributeCategories)
                .HasForeignKey(mapping => mapping.AttributeCategoryId)
                .IsRequired();

        }
    }
}
