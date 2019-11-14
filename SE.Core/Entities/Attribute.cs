﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Attribute:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AttributeCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public AttributeCategory AttributeCategory { get; set; }
        public ICollection<AttributeEducation> AttributeEducations { get; set; }
    }
}
