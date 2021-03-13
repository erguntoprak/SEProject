using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SE.Business.Helpers;
using SE.Core.Aspects.Autofac.Caching;
using SE.Core.DTO;
using SE.Core.Entities;
using SE.Core.Utilities.Results;
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
        [CacheAspect]
        public async Task<IDataResult<AddressDto>> GetCityNameDistrictsAsync()
        {
                AddressDto addressDto = await _unitOfWork.CityRepository.Include(d => d.Districts).Select(d => new AddressDto {
                    CityDto = new CityDto {
                        Id = d.Id,
                        Name = d.Name
                    },
                    DistrictListDto = d.Districts.Select(c=> new DistrictDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        SeoUrl = c.SeoUrl
                    }).OrderBy(d=>d.Name).ToList()
                }).FirstOrDefaultAsync();
            return new SuccessDataResult<AddressDto>(addressDto);
        }
    }
}
