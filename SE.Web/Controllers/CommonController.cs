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
    [EnableCors("ApiPolicy")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;

        public CommonController(ICommonService commonService, IMapper mapper)
        {
            _commonService = commonService;
            _mapper = mapper;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData([FromQuery]DashboardFilterModel dashboardFilterModel)
        {
            var result = await _commonService.GetDashboardDataAsync(_mapper.Map<DashboardFilterDto>(dashboardFilterModel));
            if (result.Success)
                return Ok(_mapper.Map<DashboardDataModel>(result.Data));
            return BadRequest(result.Message);
        }

    }
}