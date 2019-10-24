using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationAttributeEducationMap : IEntityTypeConfiguration<EducationAttributeEducation>
    {
        public void Configure(EntityTypeBuilder<EducationAttributeEducation> builder)
        {
            builder.ToTable("EducationAttributeEducation");

            builder.HasKey(x => x.Id);

            builder.HasOne(mapping => mapping.Education)
                 .WithMany(discount => discount.EducationAttributeEducations)
                 .HasForeignKey(mapping => mapping.EducationId)
                 .IsRequired();

            builder.HasOne(mapping => mapping.EducationAttribute)
                .WithMany(category => category.EducationAttributeEducations)
                .HasForeignKey(mapping => mapping.EducationAttributeId)
                .IsRequired();

        }
    }
}
