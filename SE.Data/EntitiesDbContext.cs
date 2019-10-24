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
            modelBuilder.ApplyConfiguration(new EducationAttributeCategoryMap());
            modelBuilder.ApplyConfiguration(new EducationAttributeEducationMap());
            modelBuilder.ApplyConfiguration(new EducationAttributeMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
     
        }
        public DbSet<Education> Education { get; set; }
        public DbSet<EducationAttribute> EducationAttribute { get; set; }
        public DbSet<EducationAttributeCategory> EducationAttributeCategory { get; set; }
        public DbSet<EducationAttributeEducation> EducationAttributeEducation { get; set; }
        public DbSet<Category> Category { get; set; }


    }
}
