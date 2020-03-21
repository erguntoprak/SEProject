using System.Collections.Generic;

namespace SE.Core.DTO
{
    public class AddressDto
    {
        public CityDto CityDto { get; set; }
        public List<DistrictDto> DistrictListDto { get; set; }
    }
}
