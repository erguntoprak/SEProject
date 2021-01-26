using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class EducationMap : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.ToTable("Education");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(400).IsRequired();
            builder.Property(x => x.SeoUrl).HasMaxLength(420).IsRequired();
            builder.Property(d => d.UserId).HasMaxLength(50).IsRequired();

            builder.Property(d => d.AuthorizedName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.AuthorizedEmail).HasMaxLength(200).IsRequired();
            builder.Property(d => d.PhoneOne).HasMaxLength(20).IsRequired();
            builder.Property(d => d.PhoneTwo).HasMaxLength(20);

            builder.Property(d => d.Email).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Website).HasMaxLength(200);

            builder.Property(d => d.YoutubeVideoOne).HasMaxLength(300);
            builder.Property(d => d.YoutubeVideoTwo).HasMaxLength(300);
            builder.Property(d => d.FacebookAccountUrl).HasMaxLength(300);
            builder.Property(d => d.InstagramAccountUrl).HasMaxLength(300);
            builder.Property(d => d.TwitterAccountUrl).HasMaxLength(300);
            builder.Property(d => d.YoutubeAccountUrl).HasMaxLength(300);
            builder.Property(d => d.MapCode).HasMaxLength(1000);

            builder.Property(d => d.IsActive).IsRequired();

            builder.HasOne(d => d.EducationAddress).WithOne(e => e.Education).HasForeignKey<EducationAddress>(f => f.EducationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Images).WithOne(d => d.Education).HasForeignKey(d => d.EducationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Questions).WithOne(d => d.Education).HasForeignKey(d => d.EducationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.EducationContactForms).WithOne(d => d.Education).HasForeignKey(d => d.EducationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
