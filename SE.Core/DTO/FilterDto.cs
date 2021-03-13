using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class FilterDto
    {
        public int CategoryId { get; set; }
        public int DistrictId { get; set; }
        public string SearchText { get; set; }
    }
}
