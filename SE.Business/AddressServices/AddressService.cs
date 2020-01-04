using System;
using System.Linq;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Business.AddressServices
{
    public class AddressService : IAddressService
    {
        private readonly IRepository<City> _cityRepo;
        private readonly IRepository<District> _districtRepo;

        public AddressService(IRepository<City> cityRepo, IRepository<District> districtRepo)
        {
            _cityRepo = cityRepo;
            _districtRepo = districtRepo;
        }
        public AddressDto GetCityNameDistricts()
        {
            try
            {
                AddressDto addressDto = _cityRepo.Include(d => d.Districts).Select(d => new AddressDto {
                    CityDto = new CityDto {
                        Id = d.Id,
                        Name = d.Name
                    },
                    DistrictDtoList = d.Districts.Select(c=> new DistrictDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToArray()
                }).FirstOrDefault();
                return addressDto;
            }
            catch
            {
                throw;
            }
        }
    }
}
