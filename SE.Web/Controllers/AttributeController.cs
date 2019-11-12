using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SE.Business.AttributeServices;
using SE.Core.DTO;

namespace SE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        [HttpGet("GetAllAttributeList")]
        public List<AttributeListDto> GetAllAttributeList()
        {
            var attributeList = _attributeService.GetAllEducationAttributeList();
            return attributeList;
        }
    }
}