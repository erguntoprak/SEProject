using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationContactFormMap : IEntityTypeConfiguration<EducationContactForm>
    {
        public void Configure(EntityTypeBuilder<EducationContactForm> builder)
        {
            builder.ToTable("EducationContactForm");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.NameSurname).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Email).HasMaxLength(200).IsRequired();
            builder.Property(d => d.PhoneNumber).HasMaxLength(20).IsRequired();

        }
    }
}
