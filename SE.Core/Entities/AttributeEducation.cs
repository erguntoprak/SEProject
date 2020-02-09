using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class AttributeEducation:BaseEntity
    {
        public int EducationId { get; set; }
        public int AttributeId { get; set; }
        public Education Education{ get; set; }
        public Attribute Attribute { get; set; }
    }
}
