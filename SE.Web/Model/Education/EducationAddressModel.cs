using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Education
{
    public class EducationAddressModel
    {
        public string Address { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
    }
}
