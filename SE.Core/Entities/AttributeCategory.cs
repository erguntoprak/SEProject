using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class AttributeCategory:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Attribute> Attributes{ get; set; }
        public ICollection<CategoryAttributeCategory> CategoryAttributeCategories { get; set; }

    }
}
