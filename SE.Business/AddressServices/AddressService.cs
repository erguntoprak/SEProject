using System;
using System.Linq;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Data;

namespace SE.Business.AddressServices
{
    public class AddressService : IAddressService
    {

        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AddressDto GetCityNameDistricts()
        {
            try
            {
                AddressDto addressDto = _unitOfWork.CityRepository.Include(d => d.Districts).Select(d => new AddressDto {
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
