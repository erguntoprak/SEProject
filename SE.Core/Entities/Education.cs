using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Education : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public ICollection<EducationAttributeEducation> EducationAttributeEducations { get; set; }

    }
}
