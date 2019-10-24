using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class EducationAttributeCategory:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<EducationAttribute> EducationAttributes{ get; set; }
    }
}
