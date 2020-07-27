using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class District:BaseEntity
    {
        public string Name { get; set; }
        public string SeoUrl { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<EducationAddress> EducationAddress { get; set; }

    }
}
