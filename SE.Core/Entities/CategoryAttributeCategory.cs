using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class CategoryAttributeCategory: BaseEntity
    {
        public int CategoryId { get; set; }
        public int AttributeCategoryId { get; set; }
        public Category Category { get; set; }
        public AttributeCategory AttributeCategory { get; set; }
    }
}
