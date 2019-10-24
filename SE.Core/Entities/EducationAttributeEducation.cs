using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class EducationAttributeEducation:BaseEntity
    {
        public int EducationId { get; set; }
        public int EducationAttributeId { get; set; }
        public bool IsSelected { get; set; }
        public Education Education{ get; set; }
        public EducationAttribute EducationAttribute { get; set; }



    }
}
