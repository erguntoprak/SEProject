﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class AttributeEducationMap : IEntityTypeConfiguration<AttributeEducation>
    {
        public void Configure(EntityTypeBuilder<AttributeEducation> builder)
        {
            builder.ToTable("AttributeEducation");
            builder.HasKey(x => x.Id);

            builder.HasOne(mapping => mapping.Education)
                 .WithMany(discount => discount.AttributeEducations)
                 .HasForeignKey(mapping => mapping.EducationId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();

            builder.HasOne(mapping => mapping.Attribute)
                .WithMany(category => category.AttributeEducations)
                .HasForeignKey(mapping => mapping.AttributeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
