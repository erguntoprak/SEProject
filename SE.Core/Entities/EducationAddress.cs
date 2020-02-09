using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.Entities
{
    public class EducationAddress:BaseEntity
    {
        public string AddressOne{ get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int EducationId { get; set; }

        public City City { get; set; }
        public District District { get; set; }
        public Education Education { get; set; }


    }
}
