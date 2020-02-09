using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SE.Core.Entities;
using SE.Data.Mapping;

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
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new DistrictMap());
            modelBuilder.ApplyConfiguration(new EducationAddressMap());
            modelBuilder.ApplyConfiguration(new ImageMap());
            modelBuilder.ApplyConfiguration(new QuestionMap());

        }
        public DbSet<Education> Education { get; set; }
        public DbSet<Attribute> Attribute { get; set; }
        public DbSet<AttributeCategory> AttributeCategory { get; set; }
        public DbSet<AttributeEducation> AttributeEducation { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryAttributeCategory> CategoryAttributeCategory { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<EducationAddress> EducationAddress { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Question> Question { get; set; }






    }
}
