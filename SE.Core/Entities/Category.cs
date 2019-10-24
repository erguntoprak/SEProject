using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Education> Educations { get; set; }

    }
}
