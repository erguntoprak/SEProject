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
using SE.Business.CommonServices;
using SE.Core.DTO;
using SE.Web.Model;
using SE.Web.Model.Address;
using SE.Web.Model.Common;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;

        public CommonController(ICommonService commonService, IMapper mapper)
        {
            _commonService = commonService;
            _mapper = mapper;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData([FromQuery]DashboardFilterModel dashboardFilterModel)
        {
            var result = await _commonService.GetDashboardDataAsync(_mapper.Map<DashboardFilterDto>(dashboardFilterModel));
            if (result.Success)
                return Ok(_mapper.Map<DashboardDataModel>(result.Data));
            return BadRequest(result);
        }
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("SendContactForm")]
        public async Task<IActionResult> SendContactForm([FromBody]ContactFormDto contactFormDto)
        {
            var result = await _commonService.SendContactFormAsync(contactFormDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}