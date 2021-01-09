using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AddressServices;
using SE.Web.Model;
using SE.Web.Model.Address;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetCityNameDistricts()
        {
            var result = await _addressService.GetCityNameDistrictsAsync();
            if (result.Success)
                return Ok(_mapper.Map<AddressModel>(result.Data));
            return BadRequest(result);
        }
    }
}