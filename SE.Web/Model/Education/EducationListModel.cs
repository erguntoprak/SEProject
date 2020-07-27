using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE.Web.Model.Education
{
    public class EducationListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySeoUrl { get; set; }
        public string DistrictName { get; set; }
        public string DistrictSeoUrl { get; set; }
        public int DistrictId { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string SeoUrl { get; set; }

    }
}
