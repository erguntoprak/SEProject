using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Address
{
    public class AddressModel
    {
        public CityModel CityModel { get; set; }
        public List<DistrictModel> DistrictListModel { get; set; }
    }
}
