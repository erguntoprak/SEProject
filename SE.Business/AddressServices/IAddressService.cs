using SE.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Business.AddressServices
{
    public interface IAddressService
    {
        AddressDto GetCityNameDistricts();
    }
}
