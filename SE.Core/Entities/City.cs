using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class City:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<District> Districts { get; set; }
        public ICollection<EducationAddress> EducationAddress { get; set; }

    }
}
