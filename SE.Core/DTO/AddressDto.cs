using SE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Core.DTO
{
    public class AddressDto
    {
        public CityDto CityDto { get; set; }
        public DistrictDto[] DistrictDtoList { get; set; }
    }
}
