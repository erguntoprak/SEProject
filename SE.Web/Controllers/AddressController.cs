using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AddressServices;
using SE.Web.Model;
using SE.Web.Model.Address;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }
        [HttpGet("GetCityNameDistricts")]
        public IActionResult GetCityNameDistricts()
        {
            try
            {
                var addressModel = _mapper.Map<AddressModel>(_addressService.GetCityNameDistricts());
                return Ok(addressModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu. Lütfen işlemi tekrar deneyiniz.");
            }
        }
    }
}