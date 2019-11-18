using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using SE.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Data
{
    public class EntitiesDbContext : IdentityDbContext<User>
    {
        public EntitiesDbContext(DbContextOptions<EntitiesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EducationMap());
            modelBuilder.ApplyConfiguration(new AttributeCategoryMap());
            modelBuilder.ApplyConfiguration(new AttributeEducationMap());
            modelBuilder.ApplyConfiguration(new AttributeMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new CategoryAttributeCategoryMap());
        }
        public DbSet<Education> Education { get; set; }
        public DbSet<Core.Entities.Attribute> Attribute { get; set; }
        public DbSet<AttributeCategory> AttributeCategory { get; set; }
        public DbSet<AttributeEducation> AttributeEducation { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryAttributeCategory> CategoryAttributeCategory { get; set; } 


    }
}
