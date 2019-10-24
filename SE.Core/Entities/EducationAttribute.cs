using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class EducationAttribute:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int EducationAttributeCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public EducationAttributeCategory EducationAttributeCategory { get; set; }
        public ICollection<EducationAttributeEducation> EducationAttributeEducations { get; set; }
    }
}
