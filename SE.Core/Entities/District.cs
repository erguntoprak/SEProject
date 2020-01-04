using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class District:BaseEntity
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<Neighbourhood> Neighbourhoods { get; set; }
    }
}
