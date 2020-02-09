using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data.Mapping
{
    public class QuestionMap : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");
            builder.HasKey(d => d.Id);
        }
    }
}
