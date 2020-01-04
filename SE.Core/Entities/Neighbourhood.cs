using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class Neighbourhood:BaseEntity
    {
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public string PostCode { get; set; }
        public District District { get; set; }
    }
}
